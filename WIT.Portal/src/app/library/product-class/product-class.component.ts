import { Component, OnInit, EventEmitter, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { HttpService } from '../../services/http.service';
import { SessionService } from '../../services/session.service';

import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { LibraryService } from '../../services/library.service';
import { ProductClass } from '../../entities/product-class.entity';

@Component({
    templateUrl: './product-class.component.html'
})

export class ProductClassComponent implements OnInit {

    public title: string = 'Product Class Maintenance';

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    @Input()
    public item: ProductClass;

    private get showUpdateButton(): Boolean {
        return this.item.productClassID > 0;
    }

    public hasUpdated: Boolean;

    constructor(
        private bsModalRef: BsModalRef,
        private route: ActivatedRoute,
        private sessionService: SessionService,
        private libraryService: LibraryService
    ) { }

    public ngOnInit() {

        this.hasUpdated = false;

        if (!this.item)
            this.item = new ProductClass();

        this.route.params.subscribe(params => {

            let id: string = params['id'];
            if (id != undefined) {
                this.getItem(parseInt(id));
            }
        });
    }

    public getItem(id: number): void {

        if (id > 0) {

            this.clearStatus();

            let item = new ProductClass();
            item.productClassID = id;
            this.libraryService.getProductClass(item).subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
                );
        }
    }

    private getOnSuccess(response: TransactionInfo) {

        let item = new ProductClass(response.data);
        if (item) {
            this.item = item;
        }
    }

    private getOnError(response: TransactionInfo) {

        let item = new ProductClass(response.data);
        if (item) {
            this.item = item;
        }

        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    public updateItem(): void {

        this.clearStatus();
        this.libraryService.updateProductClass(this.item).subscribe(
            response => this.updateOnSuccess(response),
            response => this.updateOnError(response)
            );
    }

    private updateOnSuccess(response: TransactionInfo) {

        let item = new ProductClass(response.data);
        this.item = item;

        this.alertBox.renderSuccessMessage(response.returnMessage);

        this.hasUpdated = true;
    }

    private updateOnError(response: TransactionInfo) {

        let item = new ProductClass(response.data);
        if (item) {
            this.item.validationErrors = item.validationErrors;
        }
        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    private clearStatus() {

        this.item.validationErrors = {};
        this.alertBox.clear();
    }
}

