import { Component, OnInit, EventEmitter, Input, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observer, Observable } from 'rxjs';
//import 'rxjs/add/operator/map';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { HttpService } from '../../services/http.service';
import { SessionService } from '../../services/session.service';

import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';

import { LibraryService } from '../../services/library.service';

import { TransactionInfo } from '../../entities/transaction-info.entity';
import { ProductFeature } from '../../entities/product-feature.entity';
import { ProductClass, ProductClassList } from '../../entities/product-class.entity';
import { Product } from '../../entities/product.entity';

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

    private productClasses: Observable<string[]>;
    private productClass: string;

    public updatedEvent: EventEmitter<Boolean> = new EventEmitter();

    constructor(
        public bsModalRef: BsModalRef,

        private route: ActivatedRoute,
        private sessionService: SessionService,
        private libraryService: LibraryService
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

        if (!this.item)
            this.item = new Product();

        this.route.params.subscribe(params => {

            let id: string = params['id'];
            if (id != undefined) {
                this.getItem(parseInt(id));
            }
        });

        this.productClasses = Observable
            .create((observer: Observer<string>) => observer.next(this.productClass))
            .mergeMap((filter: string) => {

                console.log(filter);

                let list = new ProductClassList();
                list.gridInfo.sortExpression = "Code";
                list.gridInfo.pageSize = 10;
                list.code = filter;
                return this.libraryService.getProductClasses(list)
                    .map((response: TransactionInfo) => response.data.items)
                    ;
            });
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

        this.updatedEvent.emit(true);
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
}

