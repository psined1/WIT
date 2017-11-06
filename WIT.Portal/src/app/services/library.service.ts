import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { HttpService } from './http.service';
import { SessionService } from './session.service';

import { ProductFeature, ProductFeatureList } from '../entities/product-feature.entity';
import { Customer, CustomerList } from '../entities/customer.entity';


@Injectable()
export class LibraryService {

    constructor(private httpService: HttpService, private sessionService: SessionService) { }

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