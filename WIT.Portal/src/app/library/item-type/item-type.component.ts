import { Component, OnInit } from '@angular/core';
import { AccordionConfig } from 'ngx-bootstrap/accordion';
import { DragulaService } from 'ng2-dragula';

import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

import { ItemValue, IItemData, ItemEntity } from '../../entities/item-field.entity';


@Component({
    templateUrl: './item-type.component.html',
    providers: [DragulaService]
})
export class ItemTypeComponent implements OnInit {

    private title = "Item Type: ";
    public item: ItemEntity = new ItemEntity();

    private modalEvents: any;
    private isDirty: Boolean;

    constructor(
        private dragulaService: DragulaService,
        private bsModalService: BsModalService,
        private bsModalRef: BsModalRef
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

    private addProp(): void {
        let prop = new ItemValue();
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

        this.isDirty = false;
    }
}
