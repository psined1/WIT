import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { HttpService } from './http.service';
import { SessionService } from './session.service';

import { ProductFeatureList, ProductFeature } from '../entities/product-feature.entity';


@Injectable()
export class LibraryService {

    constructor(private httpService: HttpService, private sessionService: SessionService) { }

    public getProductFeatures(list: ProductFeatureList): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductFeatures";
        return this.httpService.httpPostNonblocking(list, url);
    }
   
    public getProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/getProductFeature";
        return this.httpService.httpPostNonblocking(item, url);
    }

    public updateProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/updateProductFeature";
        return this.httpService.httpPost(item, url);
    }

    public deleteProductFeature(item: ProductFeature): Observable<any> {
        let url = this.sessionService.apiServer + "library/deleteProductFeature";
        return this.httpService.httpPost(item, url);
    }

}