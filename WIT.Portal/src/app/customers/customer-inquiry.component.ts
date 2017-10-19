import { Component, OnInit, EventEmitter, Output, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { DataGridColumn, DataGridButton, DataGridEventInformation } from '../shared/datagrid/datagrid.core';
import { DataGrid } from '../shared/datagrid/datagrid.component';

import { AlertService } from '../services/alert.service';
import { CustomerService } from '../services/customer.service';
import { AlertBoxComponent } from '../shared/alertbox.component';
import { Customer } from '../entities/customer.entity';
import { TransactionalInformation } from '../entities/transactionalInformation.entity';

import { CustomerMaintenanceComponent } from '../customers/customer-maintenance.component';
import { ConfirmYesNoComponent } from '../shared/confirm-yes-no/confirm-yes-no.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs/Subscription';


@Component({
    templateUrl: './customer-inquiry.component.html'
})

export class CustomerInquiryComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;

    public title: string = 'Customer Inquiry';
    public customers: Customer[];
    public columns = [];

    public alerts: Array<string> = [];
    public messageBox: string;

    public totalRows: number;
    public currentPageNumber: number = 1;
    public totalPages: number;
    public pageSize: number;
    public companyName: string;
    public customerCode: string;
    private sortDirection: string;
    private sortExpression: string;

    public autoFilter: Boolean = false;
    public delaySearch: Boolean = false;
    public runningSearch: Boolean = false;

    private modalSubscription: Subscription;
    private updatedEvent: EventEmitter<Boolean>;
    private requiresRefresh: Boolean = false;
    private modalRef: BsModalRef;


    constructor(
        private alertService: AlertService,
        private customerService: CustomerService,
        private router: Router,
        private route: ActivatedRoute,

        private modalService: BsModalService
    ) {

        this.currentPageNumber = 1;
        this.autoFilter = false;
        this.totalPages = 0;
        this.totalRows = 0;
        this.pageSize = 15;
        this.sortDirection = "ASC";
        this.sortExpression = "CompanyName";

    }

    public ngOnInit() {

        this.columns.push(new DataGridColumn('customerCode', 'Customer Code', '{"width": "20%" , "disableSorting": false}'));
        this.columns.push(new DataGridColumn('companyName', 'Company Name', '{"width": "30%" , "hyperLink": true, "disableSorting": false}'));
        this.columns.push(new DataGridColumn('cityAndState', 'City', '{"width": "20%" , "disableSorting": false}'));
        this.columns.push(new DataGridColumn('zipCode', 'Zip Code', '{"width": "10%" , "disableSorting": false}'));
        this.columns.push(new DataGridColumn('updatedOn', 'Date Updated', '{"width": "15%" , "disableSorting": false, "formatDate": true}'));
        this.columns.push(new DataGridColumn('', 'Delete', '{"buttons": [{"name": "x", "text": "x"}]}'));

        this.route.params.subscribe(params => {

            let customerCode: string = params['customerCode'];
            if (customerCode != undefined) {
                this.customerCode = customerCode;
            }
            let companyName: string = params['companyName'];
            if (companyName != undefined) {
                this.companyName = companyName;
            }
        });

        this.executeSearch();

    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        this.runningSearch = true;

        setTimeout(() => {

            let customer = new Customer();
            customer.customerCode = this.customerCode;
            customer.companyName = this.companyName;
            customer.pageSize = this.pageSize;
            customer.sortDirection = this.sortDirection;
            customer.sortExpression = this.sortExpression;
            customer.currentPageNumber = this.currentPageNumber;

            this.customerService.getCustomers(customer)
                .subscribe(
                response => this.getCustomersOnSuccess(response),
                response => this.getCustomersOnError(response));

        }, this.delaySearch ? 500 : 0)

    }


    private getCustomersOnSuccess(response: Customer): void {

        let transactionalInformation = new TransactionalInformation();
        transactionalInformation.currentPageNumber = this.currentPageNumber;
        transactionalInformation.pageSize = this.pageSize;
        transactionalInformation.totalPages = response.totalPages;
        transactionalInformation.totalRows = response.totalRows;
        transactionalInformation.sortDirection = this.sortDirection;
        transactionalInformation.sortExpression = this.sortExpression;

        this.customers = response.customers.map(c => new Customer(c));

        this.datagrid.databind(transactionalInformation);

        this.alertService.renderSuccessMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();

        this.runningSearch = false;

    }

    private getCustomersOnError(response): void {

        this.alertService.renderErrorMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();

        this.runningSearch = false;

    }

    public datagridEvent(event) {
        let datagridEvent: DataGridEventInformation = event.value;

        if (datagridEvent.EventType == "PagingEvent") {
            this.pagingCustomers(datagridEvent.CurrentPageNumber);
        }

        else if (datagridEvent.EventType == "PageSizeChanged") {
            this.pageSizeChanged(datagridEvent.PageSize);
        }

        else if (datagridEvent.EventType == "ItemSelected") {
            this.selectedCustomer(datagridEvent.ItemSelected);
        }

        else if (datagridEvent.EventType == "Sorting") {
            this.sortCustomers(datagridEvent.SortDirection, datagridEvent.SortExpression);
        }

        else if (datagridEvent.EventType == "ButtonClicked") {
            if (datagridEvent.Button.Name === "x") {
                let rowSelected = datagridEvent.ItemSelected;
                let customer = this.customers[rowSelected];
                this.onDelete(customer);
            }
        }
    }

    private onDelete(customer: Customer) {
        this.requiresRefresh = false;
        this.modalSubscription = this.modalService.onHide.subscribe((reason: string) => {
            //console.log(`onHide event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            this.modalSubscription.unsubscribe();
            if (this.modalRef.content.result === true) {
                console.log("Deleting customer " + customer.companyName);

                this.customerService.deleteCustomer(customer)
                    .subscribe(
                    response => this.executeSearch(),
                    response => this.getCustomersOnError(response));
            }
        });
        this.modalRef = this.modalService.show(ConfirmYesNoComponent);
        let yesNo: ConfirmYesNoComponent = this.modalRef.content;
        yesNo.title = "Delete Customer";
        yesNo.message = "About to delete customer account '" + customer.companyName + "'. Proceed?";
    }

    private showCustomerForm(customerID: number) {

        this.requiresRefresh = false;
        this.modalSubscription = this.modalService.onHidden.subscribe((reason: string) => {
            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            this.modalSubscription.unsubscribe();
            this.updatedEvent.unsubscribe();

            if (this.requiresRefresh) {
                this.executeSearch();
            }
        });
        let modalRef = this.modalService.show(CustomerMaintenanceComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        );
        let maintComponent: CustomerMaintenanceComponent = modalRef.content;
        this.updatedEvent = maintComponent.updatedEvent
            .subscribe(updated => this.requiresRefresh = updated)
            ;
        maintComponent.setCustomerID(customerID);
    }

    private newCustomer() {

        this.showCustomerForm(0);
    }

    private selectedCustomer(itemSelected: number) {

        let rowSelected = itemSelected;
        let row = this.customers[rowSelected];
        let customerID = row.customerID;

        //this.router.navigate(['/customers/customer-maintenance', { id: customerID }]);

        this.showCustomerForm(customerID);
    }

    private sortCustomers(sortDirection: string, sortExpression: string) {
        this.sortDirection = sortDirection;
        this.sortExpression = sortExpression;
        this.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    private pagingCustomers(currentPageNumber: number) {
        this.currentPageNumber = currentPageNumber;
        this.delaySearch = false;
        this.executeSearch();
    }

    private pageSizeChanged(pageSize: number) {
        this.pageSize = pageSize;
        this.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public reset(): void {
        this.customerCode = "";
        this.companyName = "";
        this.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public search(): void {
        this.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public companyNameChanged(newValue): void {

        if (this.autoFilter == false) return;

        this.companyName = newValue;
        this.currentPageNumber = 1;
        this.delaySearch = true;
        this.executeSearch();
    }

    public customerCodeChanged(newValue): void {

        if (this.autoFilter == false) return;

        this.customerCode = newValue;
        this.currentPageNumber = 1;
        this.delaySearch = true;
        this.executeSearch();
    }


}