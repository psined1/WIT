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
        return this.httpService.httpPost(user, url);
    }

    public Ping(user: User): Observable<any> {

        let url = this.sessionService.apiServer + "users/Ping";     
        return this.httpService.httpPost(user, url);
    }

    public login(user: User): Observable<any> {

        let url = this.sessionService.apiServer +  "users/login";
        return this.httpService.httpPost(user, url);
    }

    public authenicate(): Observable<any> {
    
        let user : User = new User();
        let url = this.sessionService.apiServer + "users/Authenicate";
        return this.httpService.httpPostNonblocking(user, url);
    }

    public getProfile(): Observable<any> {

        let user : User = new User();
        let url = this.sessionService.apiServer + "users/GetProfile";
        return this.httpService.httpPost(user, url);
    }

    public updateProfile(user: User): Observable<any> {

        let url = this.sessionService.apiServer + "users/UpdateProfile";
        return this.httpService.httpPost(user, url);
    }
}