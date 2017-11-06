import { Injectable, EventEmitter } from '@angular/core';
import { Headers } from '@angular/http';

import { User } from '../entities/user.entity';


@Injectable()
export class SessionService {
   
    private user: User;

    public get isAuthenicated(): Boolean {
        return this.user.userID !== 0;
    }

    public get currentUser(): User {
        return new User(this.user);             // return a secure copy
    }

    public sessionEvent: EventEmitter<User>;
    public apiServer: string;
    public version: string;
    
    constructor() {      
        this.sessionEvent = new EventEmitter();    
        this.user = new User();    
    }
 
    public authenicated(user: User) {   

        this.user = new User(user);   

        if (!this.isAuthenicated)
            this.saveToken("");

        this.sessionEvent.emit(this.currentUser);
    }

    public logout() {

        this.user = new User();

        this.saveToken("");
    }

    private saveToken(token: string): void {

        if (typeof (Storage) !== "undefined") {
            localStorage.setItem("WIT.Token", token);
        }
    }

    private getToken(): string {

        let token: string = "";
        if (typeof (Storage) !== "undefined") {
            token = localStorage.getItem("WIT.Token");
        }
        return token;
    }

    public setToken(headers: Headers): void {

        let token = this.getToken();
        if (token) {
            headers.append('Authorization', token);
        }
    }

    public updateToken(headers: Headers): void {

        let authorizationToken = headers.get("Authorization");
        if (authorizationToken != null && authorizationToken !== this.getToken()) {
            this.saveToken(authorizationToken);
        }
    }
}