import { Component, OnInit, EventEmitter, Output, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { DataGridColumn, DataGridButton, DataGridEventInformation } from '../../shared/datagrid/datagrid.core';
import { DataGrid } from '../../shared/datagrid/datagrid.component';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs/Subscription';

import { LibraryService } from '../../services/library.service';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { ProductFeature, ProductFeatureList } from '../../entities/product-feature.entity';
import { ProductFeatureComponent } from "./product-feature.component";


@Component({
    templateUrl: './product-feature-list.component.html'
})

export class ProductFeatureListComponent implements OnInit {

    @ViewChild(DataGrid) datagrid: DataGrid;
    @ViewChild(AlertBoxComponent) alertBox: AlertBoxComponent;

    public title: string = 'Product Features';
    public list: ProductFeatureList = new ProductFeatureList();
    public columns = [];

    public autoFilter: Boolean = false;
    public delaySearch: Boolean = false;
    public runningSearch: Boolean = false;

    private modalSubscription: Subscription;
    private updatedEvent: EventEmitter<Boolean>;
    private requiresRefresh: Boolean = false;
    private modalRef: BsModalRef;


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

        console.log('init');

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

        setTimeout(() => {

            let list = new ProductFeatureList();
            list.gridInfo = this.list.gridInfo;
            list.code = this.list.code;
            list.name = this.list.name;

            this.libraryService.getProductFeatures(list).subscribe(
                response => this.getListOnSuccess(response),
                response => this.getListOnError(response)
                );

        }, this.delaySearch ? 500 : 0);
    }


    private getListOnSuccess(response: TransactionInfo): void {

        let list = new ProductFeatureList(response.data);
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

    private onEdit(item?: ProductFeature) {

        this.requiresRefresh = false;
        this.modalSubscription = this.modalService.onHidden.subscribe((reason: string) => {
            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            this.modalSubscription.unsubscribe();
            this.updatedEvent.unsubscribe();

            if (this.requiresRefresh) {
                this.executeSearch();
            }
        });
        let modalRef = this.modalService.show(ProductFeatureComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        );
        let maintComponent: ProductFeatureComponent = modalRef.content;
        this.updatedEvent = maintComponent.updatedEvent
            .subscribe(updated => this.requiresRefresh = updated)
            ;
        if (item) {
            maintComponent.getItem(item.productFeatureId);
        }
    }

    private addItem() {

        this.onEdit();
    }

    private onItemSelected(itemSelected: number) {

        let rowSelected = itemSelected;
        let item = this.list.items[rowSelected];

        //this.router.navigate(['/customers/customer-maintenance', { id: item.ProductFeatureId }]);

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