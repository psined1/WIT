import { Component, EventEmitter, OnInit, Input, ElementRef, ApplicationRef } from '@angular/core';
import { SessionService } from './services/session.service';
import { User } from './entities/user.entity';
import { UserService } from './services/user.service';
import { CustomerService } from './services/customer.service';
import { HttpService } from './services/http.service';
import { BlockUIService } from './services/blockui.service';
import { AlertService } from './services/alert.service';
import { Router } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [UserService, CustomerService, HttpService, BlockUIService, AlertService]
})
export class AppComponent implements OnInit {

    public firstName: string;
    public lastName: string;
    public isAuthenicated: Boolean = false;
    public endDateTime: string;

    public blockUI: Boolean;

    public currentRoute: string;
    public title: string;
    public version: string;
    public webApiEndPoint: string;
    public imagesDirectory: string;

    constructor(
        private sessionService: SessionService,
        private userService: UserService,
        private blockUIService: BlockUIService,
        private router: Router,
        private elementRef: ElementRef
    ) {
        let native = this.elementRef.nativeElement;
     
        this.webApiEndPoint = native.getAttribute("webApiEndPoint");
        this.imagesDirectory = native.getAttribute("imagesDirectory");
       
        console.log("images directory=" + this.imagesDirectory);

        this.sessionService.apiServer = this.webApiEndPoint;      
    }

    public ngOnInit() {

        this.sessionService.version = this.version;

        this.sessionService.sessionEvent.subscribe(user => this.onAuthenication(user));
        this.blockUIService.blockUIEvent.subscribe(event => this.blockUnBlockUI(event));

        this.blockUIService.startBlock();

        this.userService.authenicate()
            .subscribe(
            response => this.authenicateOnSuccess(response),
            response => this.authenicateOnError(response));
    }

    private blockUnBlockUI(event) {
        this.blockUI = event.value;
    }

    private authenicateOnSuccess(response: User) {

        this.blockUIService.stopBlock();

        if (response.returnStatus == false) {
            return;
        }

        this.sessionService.authenicated(response);

        this.currentRoute = this.router.url;

        if (this.currentRoute == "/" || this.currentRoute == undefined) {
            this.router.navigate(['/home/home']);
            return;
        } else {
            this.router.navigate([this.currentRoute]);
        }
    }

    private authenicateOnError(response) {

        this.isAuthenicated = false;
        this.blockUIService.stopBlock();
    }

    private onAuthenication(user: User): void {

        this.firstName = user.firstName;
        this.lastName = user.lastName;
        this.isAuthenicated = true;
    }

    public logout() {

        this.sessionService.logout();
        this.router.navigate(['/home/home']);
    }
}
