import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
//import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { SessionService } from '../services/session.service';
import { BlockUIService } from './blockui.service';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';

import { TransactionalInformation } from '../entities/transactionalInformation.entity';


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

    public httpPost(url: string, object?: any): Observable<any> {

        this.blockUIService.startBlock();
     
        let body = JSON.stringify(object || {});
        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options)
            .map(response => this.parseResponse(response, this.blockUIService, true))
            .catch(err => this.handleError(err, this.blockUIService, true)
            );
    }


    public httpPostNonblocking(url: string, object?: any): Observable<any> {

        let body = JSON.stringify(object || {});
        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options)
            .map(response => this.parseResponse(response, this.blockUIService, false))
            .catch(err => this.handleError(err, this.blockUIService, false)
            );
    }


    private handleError(err: any, blockUIService: BlockUIService, blocking: Boolean) {

        console.log(err);

        if (blocking) {
            blockUIService.stopBlock();
        }

        let transaction: any;

        if (err.ok !== undefined && !err.ok) {
            transaction = new TransactionalInformation();   // TODO: change to TransactionInfo
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
