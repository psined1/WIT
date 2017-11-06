
import { Component, OnInit, Input, ViewChild } from '@angular/core';

import { AlertBoxComponent } from '../shared/alertbox.component';
import { UserService } from '../services/user.service';
import { HttpService } from '../services/http.service';
//import { AlertService } from '../services/alert.service';
import { SessionService } from '../services/session.service';
import { AddressComponent } from '../shared/address.component';
import { Router } from '@angular/router';

import { TransactionInfo } from '../entities/transaction-info.entity';
import { User } from '../entities/user.entity';
import { Address } from '../entities/address.entity';


@Component({
    templateUrl: './user-profile.component.html'
})

export class UserProfileComponent implements OnInit {

    public title: string = 'User Profile';  

    @Input()
    public item: User;

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    private address: Address;

    /*public messageBox: string;
    public alerts: Array<string> = [];
    public firstName: string;
    public lastName: string;*/

    /*public firstNameInputError: Boolean;
    public lastNameInputError: Boolean;*/

    constructor(
        private userService: UserService,
        private sessionService: SessionService,
        private router: Router
    ) { }

    public ngOnInit() {

        //this.address = new Address();

        // populate from session first
        /*this.firstName = this.sessionService.firstName;
        this.lastName = this.sessionService.lastName;
        this.address.addressLine1 = this.sessionService.addressLine1;
        this.address.addressLine2 = this.sessionService.addressLine2;
        this.address.city = this.sessionService.city;
        this.address.state = this.sessionService.state;
        this.address.zipCode = this.sessionService.zipCode;*/

        this.item = this.sessionService.currentUser;
        this.address = this.item.address;
        this.clearStatus();

        // update from backend
        this.userService.getProfile().subscribe(
            response => {

                let item = new User(response.data);
                if (item) {
                    this.item = item;
                    this.address = this.item.address;
                }
                
                /*this.firstName = response.firstName;
                this.lastName = response.lastName;
                this.address.addressLine1 = response.addressLine1;
                this.address.addressLine2 = response.addressLine2;
                this.address.city = response.city;
                this.address.state = response.state;
                this.address.zipCode = response.zipCode;            */
            },
            response => {

                let item = new User(response.data);
                if (item) {
                    this.item = item;
                }

                this.alertBox.renderErrorMessage(response.returnMessage);

                /*this.alertService.renderErrorMessage(response.returnMessage);
                this.messageBox = this.alertService.returnFormattedMessage();
                this.alerts = this.alertService.returnAlerts();
                this.alertService.setValidationErrors(this, response.validationErrors);*/
            }
        );
    }

    private clearStatus() {
        this.item.validationErrors = {};
        this.alertBox.clear();
    }


    public updateProfile(): void {

        /*let user = new User();
        user.firstName = this.firstName;
        user.lastName = this.lastName;
        user.addressLine1 = this.address.addressLine1;
        user.addressLine2 = this.address.addressLine2;
        user.city = this.address.city;
        user.state = this.address.state;
        user.zipCode = this.address.zipCode;*/

        this.clearStatus();

        this.userService.updateProfile(this.item).subscribe(
            response => this.updateProfileSuccess(response),
            response => this.updateProfileOnError(response)
        );

    }

    private updateProfileSuccess(response: TransactionInfo): void {

        this.sessionService.authenicated(response.data);

        this.item = this.sessionService.currentUser;

        this.alertBox.renderSuccessMessage(response.returnMessage);

        /*this.alertService.renderSuccessMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();

        this.sessionService.firstName = this.firstName;
        this.sessionService.lastName = this.lastName;
        this.sessionService.addressLine1 = this.address.addressLine1;
        this.sessionService.addressLine2 = this.address.addressLine2;
        this.sessionService.city = this.address.city;
        this.sessionService.state = this.address.state;
        this.sessionService.zipCode = this.address.zipCode;           
         */
    }

    private updateProfileOnError(response: TransactionInfo): void {

        let item = new User(response.data);
        if (item) {
            this.item.validationErrors = item.validationErrors;
        }
        this.alertBox.renderErrorMessage(response.returnMessage);

        /*this.alertService.renderErrorMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();
        this.alertService.setValidationErrors(this, response.validationErrors);  */  
    }
}