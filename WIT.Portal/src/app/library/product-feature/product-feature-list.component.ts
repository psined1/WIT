import { Component, OnInit, EventEmitter, Output, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { DataGridColumn, DataGridButton, DataGridEventInformation } from '../../shared/datagrid/datagrid.core';
import { DataGrid } from '../../shared/datagrid/datagrid.component';

import { AlertService } from '../../services/alert.service';
import { LibraryService } from '../../services/library.service';
import { AlertBoxComponent } from '../../shared/alertbox.component';
//import { Customer } from '../entities/customer.entity';
//import { TransactionalInformation } from '../entities/transactionalInformation.entity';

//import { CustomerMaintenanceComponent } from '../customers/customer-maintenance.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs/Subscription';

import { ProductFeature, ProductFeatureList } from '../../entities/product-feature.entity';
import { TransactionInfo } from '../../entities/transaction-info.entity';


@Component({
    templateUrl: './product-feature-list.component.html'
})

export class ProductFeatureListComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;

    public title: string = 'Product Features';
    public list: ProductFeatureList = new ProductFeatureList();
    public columns = [];

    //public alerts: Array<string> = [];
    //public messageBox: string;

    //public totalRows: number;
    //public currentPageNumber: number = 1;
    //public totalPages: number;
    //public pageSize: number;
    public name: string;
    public code: string;
    //private sortDirection: string;
    //private sortExpression: string;

    public autoFilter: Boolean = false;
    public delaySearch: Boolean = false;
    public runningSearch: Boolean = false;

    private modalSubscription: Subscription;
    private updatedEvent: EventEmitter<Boolean>;
    private requiresRefresh: Boolean = false;
    private modalRef: BsModalRef;


    constructor(
        private alertService: AlertService,
        private libraryService: LibraryService,
        private router: Router,
        private route: ActivatedRoute,
        private modalService: BsModalService
    ) {
        this.autoFilter = false;
        this.list.gridInfo.pageSize = 20;
    }

    public ngOnInit() {

        console.log('init');

        this.columns.push(new DataGridColumn('code', 'Feature Code', '{"width": "20%"}'));
        this.columns.push(new DataGridColumn('name', 'Feature Name', '{"width": "30%" , "hyperLink": true}'));
        this.columns.push(new DataGridColumn('description', 'Description', '{"width": "30%"}'));
        this.columns.push(new DataGridColumn('updatedOn', 'Updated On', '{"width": "15%" , "formatDate": true}'));
        this.columns.push(new DataGridColumn('', '', '{"disableSorting": true, "buttons": [{"name": "x", "icon": "trash", "class": "btn btn-danger"}]}'));

        this.route.params.subscribe(params => {

            let code: string = params['code'];
            if (code != undefined) {
                this.code = code;
            }
            let name: string = params['name'];
            if (name != undefined) {
                this.name = name;
            }
        });

        this.executeSearch();
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        this.runningSearch = true;

        setTimeout(() => {

            this.libraryService.getProductFeatures(this.list)
                .subscribe(
                response => this.getListOnSuccess(response),
                response => this.getListOnError(response)
                );

        }, this.delaySearch ? 500 : 0);
    }


    private getListOnSuccess(response: TransactionInfo): void {

        this.list = new ProductFeatureList(response.data);
        this.datagrid.databind(this.list.gridInfo);
        this.alertService.renderSuccessMessage(response.returnMessage);
        this.runningSearch = false;
    }

    private getListOnError(response: TransactionInfo): void {

        this.alertService.renderErrorMessage(response.returnMessage);
        this.runningSearch = false;
    }

    public datagridEvent(event) {

        let datagridEvent: DataGridEventInformation = event.value;

        if (datagridEvent.EventType == "PagingEvent") {
            this.onPagingEvent(datagridEvent.CurrentPageNumber);
        }

        else if (datagridEvent.EventType == "PageSizeChanged") {
            this.onPageSizeChanged(datagridEvent.PageSize);
        }

        else if (datagridEvent.EventType == "ItemSelected") {
            //this.onItemSelected(datagridEvent.ItemSelected);
        }

        else if (datagridEvent.EventType == "Sorting") {
            this.onSorting(datagridEvent.SortDirection, datagridEvent.SortExpression);
        }

        else if (datagridEvent.EventType == "ButtonClicked") {
            if (datagridEvent.Button.Name === "x") {
                let i = datagridEvent.ItemSelected;
                let item = this.list.items[i];
                this.onDelete(item);
            }
        }
    }

    
    private onDelete(item: ProductFeature) {
        this.requiresRefresh = false;
        this.modalSubscription = this.modalService.onHide.subscribe((reason: string) => {
            //console.log(`onHide event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            this.modalSubscription.unsubscribe();
            if (this.modalRef.content.result) {
                console.log("Deleting product feature " + item.code);

                this.libraryService.deleteProductFeature(item)
                    .subscribe(
                    response => this.executeSearch(),
                    response => this.getListOnError(response)
                    );
            }
        });
        this.modalRef = this.modalService.show(ConfirmYesNoComponent);
        let yesNo: ConfirmYesNoComponent = this.modalRef.content;
        yesNo.title = "Delete Product Feature";
        yesNo.message = "About to delete product feature '" + item.code + "'. Proceed?";
    }

    /*private onEdit(index: number) {

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
    }*/

    /*private newCustomer() {

        this.onEdit(0);
    }

    private onItemSelected(itemSelected: number) {

        let rowSelected = itemSelected;
        let row = this.customers[rowSelected];
        let customerID = row.customerID;

        //this.router.navigate(['/customers/customer-maintenance', { id: customerID }]);

        this.onEdit(customerID);
    }*/

    private onSorting(sortDirection: string, sortExpression: string) {
        this.list.gridInfo.sortDirection = sortDirection;
        this.list.gridInfo.sortExpression = sortExpression;
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    private onPagingEvent(page: number) {
        this.list.gridInfo.currentPageNumber = page;
        this.delaySearch = false;
        this.executeSearch();
    }

    private onPageSizeChanged(pageSize: number) {
        this.list.gridInfo.pageSize = pageSize;
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public reset(): void {
        this.code = "";
        this.name = "";
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public search(): void {
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public nameChanged(newValue): void {

        if (!this.autoFilter) return;

        this.name = newValue;
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = true;
        this.executeSearch();
    }

    public codeChanged(newValue): void {

        if (!this.autoFilter) return;

        this.code = newValue;
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = true;
        this.executeSearch();
    }
}