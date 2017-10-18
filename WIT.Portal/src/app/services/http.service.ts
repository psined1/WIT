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
    ) {
    }

    private makeHeaders(): Headers {
        let headers = new Headers();
        headers.append("Content-Type", "application/json; charset=utf-8");
        headers.append('Accept', 'q=0.8;application/json;q=0.9');

        this.sessionService.setToken(headers);

        return headers;
    }

    public httpPost(object: any, url: string): Observable<any> {

        this.blockUIService.startBlock();
     
        let body = JSON.stringify(object);
        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options)
            .map((response) => this.parseResponse(response, this.blockUIService, true))
            .catch((err) => this.handleError(err, this.blockUIService, true));
    }


    public httpPostNonblocking(object: any, url: string): Observable<any> {

        let body = JSON.stringify(object);
        let headers = this.makeHeaders();
        let options = new RequestOptions({ headers: headers });

        return this.http.post(url, body, options)
            .map((response) => this.parseResponse(response, this.blockUIService, false))
            .catch((err) => this.handleError(err, this.blockUIService, false));
    }


    private handleError(err: any, blockUIService: BlockUIService, blocking: Boolean) {

        if (blocking) {
            blockUIService.stopBlock();
        }

        let body: any;

        if (err.status === 0) {
            body = new TransactionalInformation();
            body.returnStatus = false;
            if (err.mesage) {
                body.returnMessage = err.message;
            } else {
                switch (err.type) {
                    case 3:
                        body.returnMessage = "Server did not respond or general network error. Please try again later.";
                        break;

                    default:
                        body.returnMessage = "Fatal communication error. Contact the administrator.";
                        break;
                }
            }
        } else {
            body = err.json();
        }

        return Observable.throw(body);
    }

    private parseResponse(response: Response, blockUIService: BlockUIService, blocking: Boolean) {

        this.sessionService.updateToken(response.headers);

        if (blocking) {
            blockUIService.stopBlock();
        }
     
        let body = response.json();

        return body;
    }
}
