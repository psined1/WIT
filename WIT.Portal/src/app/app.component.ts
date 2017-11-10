import { Component, OnInit, Input, ElementRef, ApplicationRef } from '@angular/core';
import { SessionService } from './services/session.service';
import { UserService } from './services/user.service';
import { BlockUIService } from './services/blockui.service';
import { Router } from '@angular/router';
//import { environment } from '../environments/environment';

import { User } from './entities/user.entity';
import { TransactionInfo } from './entities/transaction-info.entity';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [UserService]
})
export class AppComponent implements OnInit {

    public firstName: string;
    public lastName: string;

    private get isAuthenicated(): Boolean {
        return this.sessionService.isAuthenicated;
    }

    public blockUI: Boolean;

    //public currentRoute: string;    // TODO: move to session?
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

        this.sessionService.authenticatedEvent.subscribe(event => this.onAuthenication(event.value));
        this.blockUIService.blockUIEvent.subscribe(event => this.blockUI = event.value);

        this.blockUIService.startBlock();

        //////////////////////////////////////// PROD BEGIN ////////////////////////////////////////
        this.userService.authenicate().subscribe(
            response => this.authenicateOnSuccess(response),
            response => this.authenicateOnError(response)
            );
        ////////////////////////////////////////// PROD END ////////////////////////////////////////

        //////////////////////////////////////// DEBUG BEGIN ///////////////////////////////////////
        //let usr = new User(); usr.isAuthenicated = true; usr.firstName = "Denis"; this.authenicateOnSuccess(usr);
        ////////////////////////////////////////// DEBUG END ///////////////////////////////////////
    }

    private authenicateOnSuccess(response: TransactionInfo) {

        this.blockUIService.stopBlock();

        this.sessionService.authenicated(response.data);

        let currentRoute = this.router.url;

        console.log(currentRoute);

        if (currentRoute == "/" || currentRoute == undefined) {
            this.router.navigate(['/home/home']);
        } else {
            this.router.navigate([currentRoute]);
        }
    }

    private authenicateOnError(response: TransactionInfo) {

        this.blockUIService.stopBlock();

        this.sessionService.authenicated(response.data);
    }

    private onAuthenication(user: User): void {

        this.firstName = user.firstName;
        this.lastName = user.lastName;
    }

    public logout() {

        this.sessionService.logout();
        this.router.navigate(['/home/home']);
    }
}
