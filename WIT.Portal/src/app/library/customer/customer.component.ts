import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { HttpService } from '../../services/http.service';
//import { AlertService } from '../../services/alert.service';
import { SessionService } from '../../services/session.service';

import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { LibraryService } from '../../services/library.service';
import { Customer } from '../../entities/customer.entity';
import { Address } from '../../entities/address.entity';
import { AddressComponent } from '../../shared/address.component';

@Component({
    templateUrl: './customer.component.html'
})

export class CustomerComponent implements OnInit {

    public title: string = 'Customer Maintenance';

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    @Input()
    public item: Customer;

    private address: Address;

    public get showUpdateButton(): Boolean {
        return this.item.customerID > 0;
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
            this.item = new Customer();

        if (!this.address)
            this.address = this.item.address;

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

            let item = new Customer();
            item.customerID = id;
            this.libraryService.getCustomer(item)
                .subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
                );
        }
    }

    private getOnSuccess(response: TransactionInfo) {

        let item = new Customer(response.data);
        if (item) {
            this.item = item;
            this.address = this.item.address;
        }
    }

    private getOnError(response: TransactionInfo) {

        let item = new Customer(response.data);
        if (item) {
            this.item = item;
        }

        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    public updateItem(): void {

        this.clearStatus();

        this.item.address = this.address;

        console.log(this.item);

        this.libraryService.updateCustomer(this.item)
            .subscribe(
            response => this.updateOnSuccess(response),
            response => this.updateOnError(response)
            );
    }

    private updateOnSuccess(response: TransactionInfo) {

        let item = new Customer(response.data);
        this.item = item;

        this.alertBox.renderSuccessMessage(response.returnMessage);

        this.hasUpdated = true;
    }

    private updateOnError(response: TransactionInfo) {

        let item = new Customer(response.data);
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

