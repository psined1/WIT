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
//import { ProductFeature, ProductFeatureList } from '../../entities/product-feature.entity';

import { ItemComponent } from "./item.component";

import { GridInfo, ItemGrid, IItemData } from '../../entities/grid-info.entity';
import { ItemField, LPropTypeEnum } from '../../entities/item-field.entity';




@Component({
    templateUrl: './item-list.component.html'
})

export class ItemListComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;
    @ViewChild(AlertBoxComponent) alertBox: AlertBoxComponent;

    private __title: string = 'Library items: ';
    public title: string;
    public list: ItemGrid = new ItemGrid();
    public columns: Array<DataGridColumn> = [];

    public autoFilter: Boolean = false;
    public delaySearch: Boolean = false;
    public runningSearch: Boolean = false;

    private modalSubscription: Subscription;
    private modalRef: BsModalRef;


    constructor(
        private libraryService: LibraryService,
        private router: Router,
        private route: ActivatedRoute,
        private modalService: BsModalService
    ) {
        this.autoFilter = false;
        this.list.gridInfo.pageSize = 20;
        this.list.gridInfo.itemTypeId = 2;
        this.title = this.__title;
    }

    public ngOnInit() {

        this.route.params.subscribe(params => {

            let filter: string = params['filter'];
            if (filter != undefined) {
                this.list.gridInfo.filter = filter;
            }
            let itemTypeId: number = Number.parseInt(params['itemTypeId']);
            if (!Number.isNaN(itemTypeId)) {
                this.list.gridInfo.itemTypeId = itemTypeId;
            }
            let pageSize: number = Number.parseInt(params['pageSize']);
            if (!Number.isNaN(pageSize)) {
                this.list.gridInfo.pageSize = pageSize;
            }
            let currentPage: number = Number.parseInt(params['currentPage']);
            if (!Number.isNaN(currentPage)) {
                this.list.gridInfo.currentPageNumber = currentPage;
            }
        });

        this.executeSearch();
    }

    private executeSearch(): void {

        if (this.runningSearch) return;

        this.runningSearch = true;

        setTimeout(() => {

            this.libraryService.getItems(this.list.gridInfo).subscribe(
                response => this.getListOnSuccess(response),
                response => this.getListOnError(response)
            );

        }, this.delaySearch ? 500 : 0);
    }


    private getListOnSuccess(response: TransactionInfo): void {

        //console.log(response.data);

        let list = new ItemGrid(response.data);

        // columns
        let columns: Array<DataGridColumn> = [];
        for (const c of list.fields) {

            if (!c.gridHide) {

                let options: string = '';

                if (c.key === 'key' || c.propType === LPropTypeEnum.Hyperlink) {
                    options = options + (options === '' ? '' : ',') + '"hyperLink": true';
                }

                if (!c.isSortable) {
                    options = options + (options === '' ? '' : ',') + '"disableSorting": true';
                }

                switch (c.propType) {
                    case LPropTypeEnum.Image:
                    case LPropTypeEnum.Video:
                    case LPropTypeEnum.Text:
                        //options = options + (options === '' ? '' : ',') + '"disableSorting": true';
                        break;

                    case LPropTypeEnum.Date:
                        options = options + (options === '' ? '' : ',') + '"formatDate": true';
                        break;

                    case LPropTypeEnum.Time:
                        options = options + (options === '' ? '' : ',') + '"formatTime": true';
                        break;

                    case LPropTypeEnum.DateTime:
                        options = options + (options === '' ? '' : ',') + '"formatDate": true';
                        options = options + (options === '' ? '' : ',') + '"formatTime": true';
                        break;

                    case LPropTypeEnum.Item:
                    case LPropTypeEnum.Image:
                    case LPropTypeEnum.Video:
                    case LPropTypeEnum.Hyperlink:
                        break;
                }

                //console.log(options);

                let column = new DataGridColumn(c.key, c.name, '{' + options + '}');
                columns.push(column);
            }
        }

        columns.push(new DataGridColumn('', '', '{"disableSorting": true, "buttons": [{"name": "x", "icon": "trash", "class": "btn btn-danger"}]}'));

        this.columns = columns;
        this.list = list;
        this.datagrid.databind(this.list.gridInfo);

        this.title = this.__title + this.list.gridInfo.name;

        //console.log(this.columns);
        //console.log(this.list.data);
        //console.log(this.list.gridInfo);

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
                let item = this.list.data[i];
                this.onDelete(item);
            }
        }
    }


    private onDelete(item: IItemData) {

        let yesNo: ConfirmYesNoComponent = this.modalService.show(ConfirmYesNoComponent).content;
        yesNo.title = "Delete " + this.list.gridInfo.name;
        yesNo.message = "About to delete entry '" + item.key + "'. Proceed?";

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHide event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            if (yesNo.result) {
                console.log("Deleting item " + item.id);

                this.libraryService.deleteItem(item).subscribe(
                    response => this.executeSearch(),
                    response => this.getListOnError(response)
                );
            }
        });

    }

    private onEdit(item?: IItemData) {

        console.log(item);

        let maintComponent: ItemComponent = this.modalService.show(ItemComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        ).content;

        if (item) {
            //maintComponent.getItem(item.id);
        }

        let modalHide = this.modalService.onHide.subscribe((reason: string) => {

            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            modalHide.unsubscribe();

            //if (maintComponent.hasUpdated) {
            //    this.executeSearch();
            //}
        });

    }

    private addItem() {

        this.onEdit();
    }

    private onItemSelected(itemSelected: number) {

        let rowSelected = itemSelected;
        let item = this.list.data[rowSelected];

        //this.router.navigate(['/customers/customer-maintenance', { id: item.ProductFeatureId }]);

        this.onEdit(item);
    }

    private onSorting(sortDirection: string, sortExpression: string) {
        this.list.gridInfo.sortDirection = sortDirection;
        this.list.gridInfo.sortExpression = sortExpression; // obsolete
        //this.list.fields.filter(f => f.key == sortExpression).forEach(f => this.list.gridInfo.sortId = f.id);

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
        this.list.gridInfo.reset();
        this.datagrid.sortColumn = "";
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