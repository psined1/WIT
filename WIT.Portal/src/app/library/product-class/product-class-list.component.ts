import { Component, OnInit, Output, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { DataGridColumn, DataGridButton, DataGridEventInformation } from '../../shared/datagrid/datagrid.core';
import { DataGrid } from '../../shared/datagrid/datagrid.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs/Subscription';

import { LibraryService } from '../../services/library.service';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { ProductClass, ProductClassList } from '../../entities/product-class.entity';
import { ProductClassComponent } from "./product-class.component";


@Component({
    templateUrl: './product-class-list.component.html'
})

export class ProductClassListComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;
    @ViewChild(AlertBoxComponent) alertBox: AlertBoxComponent;

    public title: string = 'Product Classes';
    public list: ProductClassList = new ProductClassList();
    public columns = [];

    public autoFilter: Boolean = false;
    public delaySearch: Boolean = false;
    public runningSearch: Boolean = false;

    //private modalSubscription: Subscription;
    //private modalRef: BsModalRef;


    constructor(
        private libraryService: LibraryService,
        private router: Router,
        private route: ActivatedRoute,
        private modalService: BsModalService
    ) {
        this.autoFilter = false;
        this.list.gridInfo.pageSize = 20;
    }

    public ngOnInit() {

        this.columns.push(new DataGridColumn('code', 'Code', '{"width": "20%"}'));
        this.columns.push(new DataGridColumn('name', 'Name', '{"width": "30%" , "hyperLink": true}'));
        this.columns.push(new DataGridColumn('description', 'Description', '{"width": "30%"}'));
        this.columns.push(new DataGridColumn('updatedOn', 'Updated On', '{"width": "15%" , "formatDate": true}'));
        this.columns.push(new DataGridColumn('', '', '{"disableSorting": true, "buttons": [{"name": "x", "icon": "trash", "class": "btn btn-danger"}]}'));

        this.route.params.subscribe(params => {

            let code: string = params['code'];
            if (code != undefined) {
                this.list.code = code;
            }
            let name: string = params['name'];
            if (name != undefined) {
                this.list.name = name;
            }
        });

        this.executeSearch();
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        this.runningSearch = true;

        this.libraryService.getItems().subscribe(response => console.log(response), response => console.log(response));

        setTimeout(() => {

            let list = new ProductClassList();
            list.gridInfo = this.list.gridInfo;
            list.code = this.list.code;
            list.name = this.list.name;

            this.libraryService.getProductClasses(list).subscribe(
                response => this.getListOnSuccess(response),
                response => this.getListOnError(response)
                );

        }, this.delaySearch ? 500 : 0);
    }


    private getListOnSuccess(response: TransactionInfo): void {

        let list = new ProductClassList(response.data);
        if (list) {
            this.datagrid.databind(list.gridInfo);
            this.list = list;
        }

        this.alertBox.renderSuccessMessage(response.returnMessage);
        this.runningSearch = false;
    }

    private getListOnError(response: TransactionInfo): void {

        this.alertBox.renderErrorMessage(response.returnMessage);
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
            this.onItemSelected(datagridEvent.ItemSelected);
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

    
    private onDelete(item: ProductClass) {

        let yesNo: ConfirmYesNoComponent = this.modalService.show(ConfirmYesNoComponent).content;
        yesNo.title = "Delete Product Class";
        yesNo.message = "About to delete product class '" + item.code + "'. Proceed?";

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHide event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            if (yesNo.result) {
                console.log("Deleting product class " + item.code);

                this.libraryService.deleteProductClass(item).subscribe(
                    response => this.executeSearch(),
                    response => this.getListOnError(response)
                    );
            }
        });
    }

    private onEdit(item?: ProductClass) {

        let maintComponent: ProductClassComponent = this.modalService.show(ProductClassComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        ).content;

        if (item) {
            maintComponent.getItem(item.productClassID);
        }

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            if (maintComponent.hasUpdated) {
                this.executeSearch();
            }
        });

    }

    private addItem() {

        this.onEdit();
    }

    private onItemSelected(itemSelected: number) {

        let rowSelected = itemSelected;
        let item = this.list.items[rowSelected];

        //this.router.navigate(['/customers/customer-maintenance', { id: item.ProductClassId }]);

        this.onEdit(item);
    }

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
        this.list.code = "";
        this.list.name = "";
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

        this.list.name = newValue;
        this.list.gridInfo.currentPageNumber = 1;

        if (this.autoFilter) {
            this.delaySearch = true;
            this.executeSearch();
        }
    }

    public codeChanged(newValue): void {

        this.list.code = newValue;
        this.list.gridInfo.currentPageNumber = 1;

        if (this.autoFilter) {
            this.delaySearch = true;
            this.executeSearch();
        }
    }
}