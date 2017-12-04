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
//import { ProductClass, ProductClassList } from '../../entities/product-class.entity';
//import { ProductClassComponent } from "./product-class.component";

import { ItemTypeComponent } from './item-type.component';

import { ItemType } from '../../entities/item-field.entity';
import { ItemTypeList } from '../../entities/grid-info.entity';


@Component({
    templateUrl: './item-type-list.component.html'
})

export class ItemTypeListComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;
    @ViewChild(AlertBoxComponent) alertBox: AlertBoxComponent;

    public title: string = 'Library: Item Types';
    public list: ItemTypeList = new ItemTypeList();
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

        this.columns.push(new DataGridColumn('name', 'Name', '{"hyperLink": true}'));
        this.columns.push(new DataGridColumn('help', 'Description', '{}'));
        this.columns.push(new DataGridColumn('updatedOn', 'Updated On', '{"formatDate": true}'));
        this.columns.push(new DataGridColumn('', '', '{"disableSorting": true, "buttons": [{"name": "x", "icon": "trash", "class": "btn btn-danger"},{"name": "e", "icon": "pencil", "class": "btn btn-primary"}]}'));

        this.route.params.subscribe(params => {

            let filter: string = params['filter'];
            if (filter != undefined) {
                this.list.gridInfo.filter = filter;
            }
        });

        this.executeSearch();
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        this.runningSearch = true;

        setTimeout(() => {

            this.libraryService.getItemTypes(this.list.gridInfo).subscribe(
                response => this.getListOnSuccess(response),
                response => this.getListOnError(response)
                );

        }, this.delaySearch ? 500 : 0);
    }

    private getListOnSuccess(response: TransactionInfo): void {

        let list = new ItemTypeList(response.data);
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

            let i = datagridEvent.ItemSelected;
            let item = this.list.items[i];

            switch (datagridEvent.Button.Name) {
                case 'x':
                    this.onDelete(item);
                    break;

                case 'e':
                    this.onEdit(item);
                    break;
            }
        }
    }

    private onDelete(item: ItemType) {

        let yesNo: ConfirmYesNoComponent = this.modalService.show(ConfirmYesNoComponent).content;
        yesNo.title = "Delete item type";
        yesNo.message = "About to delete item type '" + item.name + "'. Proceed?";

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHide event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            if (yesNo.result) {
                console.log("Deleting item type " + item.name);

                this.libraryService.deleteItemType(item.itemTypeId).subscribe(
                    response => this.executeSearch(),
                    response => this.getListOnError(response)
                    );
            }
        });
    }

    private onEdit(item?: ItemType) {

        let maintComponent: ItemTypeComponent = this.modalService.show(ItemTypeComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        ).content;

        if (item) {
            maintComponent.getItem(item.itemTypeId);
        }

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            /*if (maintComponent.hasUpdated) {
                this.executeSearch();
            }*/
        });
        
    }

    private addItem() {

        this.onEdit();
    }

    private onItemSelected(itemSelected: number) {

        let rowSelected = itemSelected;
        let item = this.list.items[rowSelected];

        this.router.navigate(['/library/item', { itemTypeId: item.itemTypeId }]);
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

        this.list.gridInfo.filter = "";
        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public search(): void {

        this.list.gridInfo.currentPageNumber = 1;
        this.delaySearch = false;
        this.executeSearch();
    }

    public filterChanged(newValue): void {

        this.list.gridInfo.filter = newValue;
        this.list.gridInfo.currentPageNumber = 1;

        if (this.autoFilter) {
            this.delaySearch = true;
            this.executeSearch();
        }
    }
}