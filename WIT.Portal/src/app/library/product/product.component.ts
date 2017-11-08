import { Component, OnInit, EventEmitter, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observer, Observable, Subscription } from 'rxjs';
//import 'rxjs/add/operator/map';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { HttpService } from '../../services/http.service';
import { SessionService } from '../../services/session.service';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { LibraryService } from '../../services/library.service';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { ProductFeature, ProductFeatureList } from '../../entities/product-feature.entity';
import { ProductClass, ProductClassList } from '../../entities/product-class.entity';
import { Product } from '../../entities/product.entity';

import { ProductClassComponent } from '../product-class/product-class.component';
import { ProductFeatureComponent } from '../product-feature/product-feature.component';

@Component({
    templateUrl: './product.component.html'
})

export class ProductComponent implements OnInit {

    public title: string = 'Product Maintenance';

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    @Input()
    public item: Product;

    public get showUpdateButton(): Boolean {
        return this.item.productID > 0;
    }

    public hasUpdated: Boolean;

    constructor(
        public bsModalRef: BsModalRef,

        private route: ActivatedRoute,
        private sessionService: SessionService,
        private libraryService: LibraryService,
        private modalService: BsModalService
    ) { }

    public getItem(id: number): void {

        if (id > 0) {

            this.clearStatus();

            let item = new Product();
            item.productID = id;

            this.libraryService.getProduct(item).subscribe(
                response => this.getOnSuccess(response),
                response => this.getOnError(response)
                );
        }
    }

    public ngOnInit() {

        this.hasUpdated = false;

        if (!this.item)
            this.item = new Product();

        this.route.params.subscribe(params => {

            let id: string = params['id'];
            if (id != undefined) {
                this.getItem(parseInt(id));
            }
        });

        this.initProductClassLookup();
        this.initProductFeatureLookup();
    }

    private getOnSuccess(response: TransactionInfo) {

        let item = new Product(response.data);
        if (item) {
            this.item = item;
        }
    }

    private getOnError(response: TransactionInfo) {

        let item = new Product(response.data);
        if (item) {
            this.item = item;
        }

        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    public updateItem(): void {

        this.clearStatus();
        this.libraryService.updateProduct(this.item).subscribe(
            response => this.updateOnSuccess(response),
            response => this.updateOnError(response)
            );
    }

    private updateOnSuccess(response: TransactionInfo) {

        let item = new Product(response.data);
        this.item = item;

        this.alertBox.renderSuccessMessage(response.returnMessage);

        this.hasUpdated = true;
    }

    private updateOnError(response: TransactionInfo) {

        let item = new Product(response.data);
        if (item) {
            this.item.validationErrors = item.validationErrors;
        }
        this.alertBox.renderErrorMessage(response.returnMessage);
    }

    private clearStatus() {

        this.item.validationErrors = {};
        this.alertBox.clear();
    }


    ////////////////////////// typeahead /////////////////////////////

    private subModalRef: BsModalRef;
    private subModalSubscription: Subscription;

    // ----------------------

    private productClasses: Observable<ProductClass[]>;

    private initProductClassLookup(): void {

        this.productClasses = Observable
            .create((observer: Observer<string>) => observer.next(this.item.productClass))
            .mergeMap((filter: string) => {

                let list = new ProductClassList();
                list.gridInfo.sortExpression = "Code";
                list.gridInfo.pageSize = 10;
                list.code = filter;
                return this.libraryService.getProductClasses(list)
                    .map((response: TransactionInfo) => response.data.items)
                    ;
            });
    }

    private productClassNoResults($event: Boolean): void {
        if ($event)
            this.item.validationErrors.productClass = "Incorrect value";
    }

    private productClassLoading($event: Boolean): void {
        if (!$event)
            this.item.validationErrors.productClass = "";
    }

    private productClassOnSelect($event: { item: ProductClass, value: string, Headers: Boolean }): void {
        this.item.productClass = $event.value;
    }

    private productClassOnBlur($event: {item: ProductClass, value: string, Headers: Boolean}): void {
        this.item.productClass = $event.value;
    }

    private addProductClass() {

        let subModalRef = this.modalService.show(ProductClassComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        );

        let maintComponent: ProductClassComponent = subModalRef.content;

        //this.subModalSubscription =
        this.modalService.onHide.subscribe((reason: string) => {
            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            //this.subModalSubscription.unsubscribe();

            console.log('triggered from product component');

            if (maintComponent.hasUpdated) {
                this.item.productClass = maintComponent.item.code;
            }
        });
    }

    // ----------------------

    private productFeatures: Observable<ProductFeature[]>;

    private initProductFeatureLookup(): void {

        this.productFeatures = Observable
            .create((observer: Observer<string>) => observer.next(this.item.productFeature))
            .mergeMap((filter: string) => {

                let list = new ProductFeatureList();
                list.gridInfo.sortExpression = "Code";
                list.gridInfo.pageSize = 10;
                list.code = filter;
                return this.libraryService.getProductFeatures(list)
                    .map((response: TransactionInfo) => response.data.items)
                    ;
            });
    }

    private productFeatureNoResults($event: Boolean): void {
        if ($event)
            this.item.validationErrors.productFeature = "Incorrect value";
    }

    private productFeatureLoading($event: Boolean): void {
        if (!$event)
            this.item.validationErrors.productFeature = "";
    }

    private productFeatureOnSelect($event: { item: ProductFeature, value: string, Headers: Boolean }): void {
        this.item.productFeature = $event.value;
    }

    private productFeatureOnBlur($event: { item: ProductFeature, value: string, Headers: Boolean }): void {
        this.item.productFeature = $event.value;
    }

    private addProducFeature() {

        let subModalRef = this.modalService.show(ProductFeatureComponent,
            Object.assign({}, {
                animated: true,
                keyboard: true,
                backdrop: true,
                ignoreBackdropClick: false
            }, { class: 'modal-lg' })
        );

        let maintComponent: ProductFeatureComponent = subModalRef.content;

        //this.subModalSubscription =
        this.modalService.onHide.subscribe((reason: string) => {
            //console.log(`onHidden event has been fired${reason ? ', dismissed by ' + reason : ''}`);
            //this.subModalSubscription.unsubscribe();

            if (maintComponent.hasUpdated) {
                this.item.productFeature = maintComponent.item.code;
            }
        });
    }
}

