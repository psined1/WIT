import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { HttpService } from '../../services/http.service';
import { SessionService } from '../../services/session.service';
import { LibraryService } from '../../services/library.service';
import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { TransactionInfo } from '../../entities/transaction-info.entity';

import { ItemField, LPropTypeEnum, ItemValue, ItemEntity } from '../../entities/item-field.entity';


@Component({
    selector: 'app-item',
    templateUrl: './item.component.html',
    styles: []
})
export class ItemComponent implements OnInit {

    public title: string = 'Item maintenance';

    private propType = LPropTypeEnum;

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    @Input()
    public item: ItemEntity = new ItemEntity();

    public hasUpdated: Boolean;

    private get showUpdateButton(): Boolean {
        return this.item.id > 0;
    }

    constructor(
        private bsModalRef: BsModalRef,
        private route: ActivatedRoute,
        private sessionService: SessionService,
        private libraryService: LibraryService
    ) {
    }

    ngOnInit() {

        this.hasUpdated = false;

        this.route.params.subscribe(params => {

            let itemId: string = params['itemId'];
            if (itemId != undefined) {
                this.getItem(parseInt(itemId));
                return;
            }

            let itemTypeId: string = params['itemTypeId'];
            if (itemTypeId != undefined) {
                this.initItem(parseInt(itemTypeId));
                return;
            }
        });
    }

    public getItem(itemId: number = 0): void {

        if (itemId > 0) {

            this.clearStatus();

            this.libraryService.getItem(itemId).subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
            );
        }
    }

    public initItem(itemTypeId: number): void {

        if (itemTypeId > 0) {

            this.clearStatus();

            this.libraryService.getBlankItem(itemTypeId).subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
            );
        }
    }

    private getOnSuccess(response: TransactionInfo) {

        this.item = new ItemEntity(response.data);
    }

    private getOnError(response: TransactionInfo) {

        this.item = new ItemEntity(response.data);
        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    public updateItem(): void {

        this.clearStatus();
        this.libraryService.updateItem(this.item).subscribe(
            response => this.updateOnSuccess(response),
            response => this.updateOnError(response)
        );
    }

    private updateOnSuccess(response: TransactionInfo) {

        this.item = new ItemEntity(response.data);
        this.alertBox.renderSuccessMessage(response.returnMessage);
        this.hasUpdated = true;
    }

    private updateOnError(response: TransactionInfo) {

        this.item = new ItemEntity(response.data);
        this.alertBox.renderErrorMessage(response.returnMessage);
    }


    private clearStatus() {

        this.item.fields.forEach(f => f.validationError == null);
        this.alertBox.clear();
    }
}
