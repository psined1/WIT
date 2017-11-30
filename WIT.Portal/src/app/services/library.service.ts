import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { HttpService } from './http.service';
import { SessionService } from './session.service';

import { ProductFeature, ProductFeatureList } from '../entities/product-feature.entity';
import { ProductClass, ProductClassList } from '../entities/product-class.entity';
import { Product, ProductList } from '../entities/product.entity';
import { Customer, CustomerList } from '../entities/customer.entity';
import { GridInfo } from '../entities/grid-info.entity';
import { ItemEntity } from '../entities/item-field.entity';


@Injectable()
export class LibraryService {

    constructor(
        private httpService: HttpService,
        private sessionService: SessionService
    ) { }

    // item type
    public getItemTypes(gridInfo: GridInfo): Observable<any> {
        let url = this.sessionService.apiServer + "library/getItemTypes";
        return this.httpService.httpPostNonblocking(url, gridInfo);
    }

    public deleteItemType(itemTypeId: number): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteItemType?itemTypeId=" + itemTypeId;
        return this.httpService.httpGet(url);
    }

    // generic item
    public getItems(gridInfo: GridInfo): Observable<any> {
        let url = this.sessionService.apiServer + "library/getItems";
        return this.httpService.httpPostNonblocking(url, gridInfo);
    }

    public deleteItem(itemId: number): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteItem?itemId=" + itemId;
        return this.httpService.httpGet(url);
    }

    public getItem(itemId: number): Observable<any> {
        let url = this.sessionService.apiServer + "library/getItem?itemId=" + itemId;
        return this.httpService.httpGet(url);
    }

    public getBlankItem(itemTypeId: number): Observable<any> {
        let url = this.sessionService.apiServer + "library/getBlankItem?itemTypeId=" + itemTypeId;
        return this.httpService.httpGet(url);
    }

    public updateItem(item: ItemEntity): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateItem";
        return this.httpService.httpPost(url, item);
    }


    // Product Features
    public getProductFeatures(list: ProductFeatureList): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductFeatures";
        return this.httpService.httpPostNonblocking(url, list);
    }

    public getProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductFeature";
        return this.httpService.httpPostNonblocking(url, item);
    }

    public updateProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateProductFeature";
        return this.httpService.httpPost(url, item);
    }

    public deleteProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteProductFeature";
        return this.httpService.httpPost(url, item);
    }

    // Product Classes
    public getProductClasses(list: ProductClassList): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductClasses";
        return this.httpService.httpPostNonblocking(url, list);
    }

    public getProductClass(item: ProductClass): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductClass";
        return this.httpService.httpPostNonblocking(url, item);
    }

    public updateProductClass(item: ProductClass): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateProductClass";
        return this.httpService.httpPost(url, item);
    }

    public deleteProductClass(item: ProductClass): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteProductClass";
        return this.httpService.httpPost(url, item);
    }

    // Products
    public getProducts(list: ProductList): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProducts";
        return this.httpService.httpPostNonblocking(url, list);
    }

    public getProduct(item: Product): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProduct";
        return this.httpService.httpPostNonblocking(url, item);
    }

    public updateProduct(item: Product): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateProduct";
        return this.httpService.httpPost(url, item);
    }

    public deleteProduct(item: Product): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteProduct";
        return this.httpService.httpPost(url, item);
    }

    // Customers
    public getCustomers(list: CustomerList): Observable<any> {
        let url = this.sessionService.apiServer + "library/getCustomers";
        return this.httpService.httpPostNonblocking(url, list);
    }

    public getCustomer(item: Customer): Observable<any> {
        let url = this.sessionService.apiServer + "library/getCustomer";
        return this.httpService.httpPostNonblocking(url, item);
    }

    public updateCustomer(item: Customer): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateCustomer";
        return this.httpService.httpPost(url, item);
    }

    public deleteCustomer(item: Customer): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteCustomer";
        return this.httpService.httpPost(url, item);
    }

}