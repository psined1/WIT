import { Component, OnInit, ViewChild, ContentChild, AfterViewInit, NgZone } from '@angular/core';
import { User } from '../entities/user.entity';

import { UserService } from '../services/user.service';
import { HttpService } from '../services/http.service';
import { SessionService } from '../services/session.service';
import { Router } from '@angular/router';
import { AlertBoxComponent } from '../shared/alertbox.component';

import { TransactionInfo } from '../entities/transaction-info.entity';

//declare var NotificationFx: any;

@Component({
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

    public title: string = "Register User Account";

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    private item: User;

    constructor(
        private userService: UserService,
        private sessionService: SessionService,
        private router: Router,
        private zone: NgZone
    ) { }

    public ngOnInit() {

        this.item = new User();

        //////////////////////////////////////// DEBUG BEGIN ///////////////////////////////////////
        this.item.firstName = "Denis";
        this.item.lastName = "Pavlichenko";
        this.item.emailAddress = "psined1@gmail.com";
        this.item.password = "test";
        this.item.passwordConfirmation = "test";
        ////////////////////////////////////////// DEBUG END ///////////////////////////////////////

        this.clearStatus();
    }

    public registerUser(): void {

        this.clearStatus();

        this.userService.registerUser(this.item).subscribe(
            response => this.registerUserOnSuccess(response),
            response => this.registerUserOnError(response)
        );
    }

    private clearStatus() {
        this.item.validationErrors = {};
        this.alertBox.clear();
    }

    private registerUserOnSuccess(response: TransactionInfo): void {

        this.sessionService.authenicated(response.data);

        this.router.navigate(['/home/home']);
    }

    private registerUserOnError(response): void {        

        let item = new User(response.data);
        if (item) {
            this.item.validationErrors = item.validationErrors;
        }

        this.alertBox.renderErrorMessage(response.returnMessage);
    }
}

