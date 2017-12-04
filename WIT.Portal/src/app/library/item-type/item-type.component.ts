import { Component, OnInit, OnChanges, ViewChild } from '@angular/core';
//import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
//import { PropTypeKeyPipe } from './prop-type-key.pipe';

import { AccordionConfig } from 'ngx-bootstrap/accordion';
import { DragulaService } from 'ng2-dragula';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SessionService } from '../../services/session.service';
import { LibraryService } from '../../services/library.service';

import { ItemField, ItemType, LPropTypeEnum } from '../../entities/item-field.entity';
import { TransactionInfo } from '../../entities/transaction-info.entity';



@Component({
    templateUrl: './item-type.component.html',
    providers: [DragulaService],
})
export class ItemTypeComponent implements OnInit, OnChanges {

    private title = "Item Type: ";
    private item: ItemType = new ItemType();

    private propTypes = LPropTypeEnum;

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    private modalEvents: any;
    private isDirty: Boolean;

    constructor(
        private dragulaService: DragulaService,
        private bsModalService: BsModalService,
        private bsModalRef: BsModalRef,
        private sessionService: SessionService,
        private libraryService: LibraryService
        //private fb: FormBuilder
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
        dragulaService.setOptions('allProps', {
            moves: function (el: any, container: any, handle: any): any {
                //console.log(el, container);
                return handle.classList.contains('handle');
            }
        });
    }

    ngOnInit() {

        /*****for (let i = 1; i <= 0; i++) {

            let item = new ObservationSheetProp();
            item.id = i;
            item.name = 'Prop ' + i;
            this.observationSheet.steps.push(item);
        }*******/

        this.isDirty = false;
    }

    ngOnChanges() {
        // TODO: change to reactive forms
    }

    public getItem(id: number): void {

        if (id > 0) {

            this.clearStatus();

            this.libraryService.getItemType(id).subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
            );
        }
    }

    private getOnSuccess(response: TransactionInfo) {

        let item = new ItemType(response.data);
        if (item) {
            this.item = item;
        }
    }

    private getOnError(response: TransactionInfo) {

        let item = new ItemType(response.data);
        if (item) {
            this.item = item;
        }

        this.alertBox.renderErrorMessage(response.returnMessage);
    }



    private addProp(): void {
        let prop = new ItemField();
        //let n = this.item.fields.length + 1;
        //prop.name = 'Property ' + n;
        this.item.fields.push(prop);

        this.isDirty = true;
    }

    private save(): void {

        /*for (let i = 0; i < this.observationSheet.steps.length; i++) {
            let prop = this.observationSheet.steps[i];
            prop.sort = i;
            console.log(prop);
        }*/

        console.log(this.item);

        this.isDirty = false;
    }

    private clearStatus() {

        //this.item.validationErrors = {};
        this.alertBox.clear();
    }
}
