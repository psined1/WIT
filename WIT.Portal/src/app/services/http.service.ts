import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
//import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { SessionService } from '../services/session.service';
import { BlockUIService } from './blockui.service';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';

import { TransactionInfo } from '../entities/transaction-info.entity';


@Injectable()
export class HttpService {

    constructor(
        private http: Http,
        private blockUIService: BlockUIService,
        private sessionService: SessionService
    ) { }

    private makeHeaders(): Headers {
        let headers = new Headers();
        headers.append("Content-Type", "application/json; charset=utf-8");
        headers.append('Accept', 'q=0.8;application/json;q=0.9');

        this.sessionService.setToken(headers);

        return headers;
    }

    public httpPost(url: string, object?: any, blockUi: boolean = true): Observable<any> {

        if (blockUi)
            this.blockUIService.startBlock();
     
        let body = JSON.stringify(object || {});
        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options)
            .map(response => this.parseResponse(response, this.blockUIService, blockUi))
            .catch(err => this.handleError(err, this.blockUIService, blockUi)
            );
    }

    public httpPostNonblocking(url: string, object?: any): Observable<any> {

        return this.httpPost(url, object, false);
    }

    public httpGet(url: string, blockUi: boolean = true): Observable<any> {

        if (blockUi)
            this.blockUIService.startBlock();

        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.get(url, options)
            .map(response => this.parseResponse(response, this.blockUIService, blockUi))
            .catch(err => this.handleError(err, this.blockUIService, blockUi)
            );
    }


    private handleError(err: any, blockUIService: BlockUIService, blocking: Boolean) {

        console.log(err);

        if (blocking) {
            blockUIService.stopBlock();
        }

        let transaction: any;

        if (err.ok !== undefined && !err.ok) {
            transaction = new TransactionInfo();
            if (err.status === 0) {
                switch (err.type) {
                    case 3:
                        transaction.returnMessage = "Server did not respond or general network error. Please try again later.";
                        break;

                    default:
                        transaction.returnMessage = "Fatal communication error type " + err.type + ". Contact the administrator.";
                        break;
                }
            } else {
                let body = err.json();
                if (body) {
                    transaction.returnMessage = body.returnMessage || body.messageDeatil || body.message;
                    transaction.data = body.data;
                } else {
                    transaction.returnMessage = err.statusText;
                }

                if (err.status === 404) {
                    this.sessionService.updateToken(err.headers);
                }
            }
        } else {
            transaction = err.json();
        }

        return Observable.throw(transaction);
    }

    private parseResponse(response: Response, blockUIService: BlockUIService, blocking: Boolean) {

        this.sessionService.updateToken(response.headers);

        if (blocking) {
            blockUIService.stopBlock();
        }
     
        let body = response.json();

        if (!body.returnMessage) {
            body.returnMessage = "As of " + new Date().toLocaleTimeString();
        }

        return body;
    }
}
