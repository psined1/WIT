import { Component, OnInit, EventEmitter } from '@angular/core';
import { AccordionConfig } from 'ngx-bootstrap/accordion';
import { DragulaService } from 'ng2-dragula';

import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';
import { StepMaintenanceComponent } from './step-maintenance.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

import { ObservationSheet } from '../../entities/observation-sheet.entity';
import { ObservationSheetStep } from '../../entities/observation-sheet-step.entity';


@Component({
    selector: 'app-observation-sheet',
    templateUrl: './observation-sheet.component.html',
    providers: [DragulaService]
})
export class ObservationSheetComponent implements OnInit {

    private title = "Observation sheet";
    public observationSheet: ObservationSheet;

    private modalRef: BsModalRef;
    private modalEvents: EventEmitter<string>;
    private isDirty: Boolean;

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
            this.isDirty = true;
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

        this.observationSheet = new ObservationSheet();

        for (let i = 1; i <= 0; i++) {

            let item = new ObservationSheetStep();
            item.id = i;
            item.name = 'Step ' + i;
            this.observationSheet.steps.push(item);
        }

        this.isDirty = false;
    }

    private addStep(): void {
        let item = new ObservationSheetStep();
        let n = this.observationSheet.steps.length + 1;
        item.name = 'Step ' + n;
        this.observationSheet.steps.push(item);

        this.isDirty = true;
    }

    private editStep(step: ObservationSheetStep): void {
        /*this.modalEvents = this.modalService.onHidden.subscribe((reason: string) => {
            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            this.modalEvents.unsubscribe();
            //this.updatedEvent.unsubscribe();

            if (this.requiresRefresh) {
                this.executeSearch();
            }
        });*/
        let modalRef = this.modalService.show(StepMaintenanceComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        );
        let maintComponent: StepMaintenanceComponent = modalRef.content;
        //this.updatedEvent = maintComponent.updatedEvent
        //    .subscribe(updated => this.requiresRefresh = updated)
        //    ;
        maintComponent.step = step;
    }

    private deleteStep(step: ObservationSheetStep): void {

        this.modalEvents = this.modalService.onHide.subscribe((reason: string) => {
            if (this.modalRef.content.result === true) {
                this.modalEvents.unsubscribe();

                /*this.customerService.deleteCustomer(customer)
                    .subscribe(
                    response => this.executeSearch(),
                    response => this.getCustomersOnError(response));*/

                let i = this.observationSheet.steps.indexOf(step);
                if (i >= 0) {
                    this.observationSheet.steps.splice(i, 1);
                    this.isDirty = true;
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

        for (let i = 0; i < this.observationSheet.steps.length; i++) {
            let step = this.observationSheet.steps[i];
            step.sort = i;
            console.log(step);
        }

        this.isDirty = false;
    }
}
