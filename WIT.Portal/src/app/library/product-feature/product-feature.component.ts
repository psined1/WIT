import { Component, OnInit, EventEmitter, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { HttpService } from '../../services/http.service';
//import { AlertService } from '../../services/alert.service';
import { SessionService } from '../../services/session.service';

import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { LibraryService } from '../../services/library.service';
import { ProductFeature } from '../../entities/product-feature.entity';

@Component({
    templateUrl: './product-feature.component.html'
})

export class ProductFeatureComponent implements OnInit {

    public title: string = 'Product Feature Maintenance';

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    @Input()
    public item: ProductFeature = new ProductFeature();

    public get showUpdateButton(): Boolean {
        return this.item.productFeatureId > 0;
    }

    public updatedEvent: EventEmitter<Boolean> = new EventEmitter();

    //public customerCodeInputError: Boolean;
    //public companyNameInputError: Boolean;

    constructor(
        public bsModalRef: BsModalRef,

        private route: ActivatedRoute,
        private sessionService: SessionService,
        //private alertService: AlertService,
        private libraryService: LibraryService
    ) { }

    public getItem(id: number): void {

        if (id > 0) {

            let item = new ProductFeature();
            item.productFeatureId = id;
            this.libraryService.getProductFeature(item)
                .subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
                );
        }
    }

    public ngOnInit() {

        this.route.params.subscribe(params => {

            let id: string = params['id'];
            if (id != undefined) {
                this.getItem(parseInt(id));
            }
        });
    }

    private getOnSuccess(response: TransactionInfo) {

        let item = new ProductFeature(response.data);
        this.alertBox.clear();
        this.item = item;
    }

    private getOnError(response: TransactionInfo) {

        this.alertBox.renderErrorMessage(response.returnMessage);
        //this.alertService.setValidationErrors(this, response.validationErrors); // TODO
    }

    public updateItem(): void {

        this.clearInputErrors();

        this.libraryService.updateProductFeature(this.item)
            .subscribe(
            response => this.updateOnSuccess(response),
            response => this.updateOnError(response)
            );
    }

    private updateOnSuccess(response: TransactionInfo) {

        let item = new ProductFeature(response.data);
        this.item = item;

        this.alertBox.renderSuccessMessage(response.returnMessage);

        this.updatedEvent.emit(true);
    }

    private updateOnError(response: TransactionInfo) {

        this.alertBox.renderErrorMessage(response.returnMessage);
        //this.alertService.setValidationErrors(this, response.validationErrors);   // TODO
    }



    private clearInputErrors() {
        //this.customerCodeInputError = false;
        //this.companyNameInputError = false;
    }
}
