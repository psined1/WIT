import { User } from '../entities/user.entity';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Headers, RequestOptions } from '@angular/http';
import { HttpService } from './http.service';
import { SessionService } from './session.service';


@Injectable()
export class UserService {

    constructor(private httpService: HttpService, private sessionService: SessionService) { }    

    public registerUser(user: User): Observable<any> {

        let url = this.sessionService.apiServer +  "users/registerUser";     
        return this.httpService.httpPost(url, user);
    }

    /*public ping(user: User): Observable<any> {

        let url = this.sessionService.apiServer + "users/ping";     
        return this.httpService.httpPost(user, url);
    }*/

    public login(user: User): Observable<any> {

        let url = this.sessionService.apiServer +  "users/login";
        return this.httpService.httpPost(url, user);
    }

    public authenicate(): Observable<any> {
    
        let url = this.sessionService.apiServer + "users/authenicate";
        return this.httpService.httpPostNonblocking(url);
    }

    public getProfile(): Observable<any> {

        let url = this.sessionService.apiServer + "users/getProfile";
        return this.httpService.httpPost(url);
    }

    public updateProfile(user: User): Observable<any> {

        let url = this.sessionService.apiServer + "users/updateProfile";
        return this.httpService.httpPost(url, user);
    }
}