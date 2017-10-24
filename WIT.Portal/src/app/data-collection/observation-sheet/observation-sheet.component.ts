import { Component, OnInit, EventEmitter } from '@angular/core';
import { ObservationSheetStep } from '../entities/observation-sheet-step.entity';
import { AccordionConfig } from 'ngx-bootstrap/accordion';
import { DragulaService } from 'ng2-dragula';

import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


@Component({
    selector: 'app-observation-sheet',
    templateUrl: './observation-sheet.component.html',
    styleUrls: ['./observation-sheet.component.css'],
    providers: [DragulaService]
})
export class ObservationSheetComponent implements OnInit {

    private title = "Observation sheet";
    public steps: Array<ObservationSheetStep>;

    private modalRef: BsModalRef;
    private modalEvents: EventEmitter<string>;
    private dirty: Boolean;

    constructor(
        private dragulaService: DragulaService,
        private modalService: BsModalService
    ) {
        dragulaService.dropModel.subscribe((value) => {
            //let [el, target, source] = value.slice(1);
            //console.log('onDropModel:');
            //console.log(el);
            //console.log(target);
            //console.log(source);
            this.dirty = true;
        });
        /*dragulaService.removeModel.subscribe((value) => {
            let [el, source] = value.slice(1);
            console.log('onRemoveModel:');
            console.log(el);
            console.log(source);
        });*/
        dragulaService.setOptions('allSteps', {
            moves: function (el: any, container: any, handle: any): any {
                //console.log(el, container);
                return handle.classList.contains('handle');
            }
        });
    }

    ngOnInit() {
        this.steps = new Array<ObservationSheetStep>();

        for (let i = 1; i <= 0; i++) {

            let item = new ObservationSheetStep();
            item.id = i;
            item.name = 'Step ' + i;
            this.steps.push(item);
        }

        this.dirty = false;
    }

    private addStep(): void {
        let item = new ObservationSheetStep();
        let n = this.steps.length + 1;
        item.name = 'Step ' + n;
        this.steps.push(item);

        this.dirty = true;
    }

    private deleteStep(step: ObservationSheetStep): void {

        this.modalEvents = this.modalService.onHide.subscribe((reason: string) => {
            if (this.modalRef.content.result === true) {
                this.modalEvents.unsubscribe();

                /*this.customerService.deleteCustomer(customer)
                    .subscribe(
                    response => this.executeSearch(),
                    response => this.getCustomersOnError(response));*/

                let i = this.steps.indexOf(step);
                if (i >= 0) {
                    //step.deleted = true;
                    this.steps.splice(i, 1);
                    this.dirty = true;
                }
            }
        });

        //console.log(step);

        this.modalRef = this.modalService.show(ConfirmYesNoComponent);
        let yesNo: ConfirmYesNoComponent = this.modalRef.content;
        yesNo.title = "Delete Observation Step";
        yesNo.message = "About to delete observation step '" + step.name + "'. Proceed?";
    }

    private save(): void {

        for (let i = 0; i < this.steps.length; i++) {
            let step = this.steps[i];
            step.sort = i;
            console.log(step);
        }

        this.dirty = false;
    }
}
