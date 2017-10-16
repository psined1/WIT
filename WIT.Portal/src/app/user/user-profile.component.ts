
import { Component, OnInit } from '@angular/core';

import { User } from '../entities/user.entity';
import { Address } from '../entities/address.entity';

import { AlertBoxComponent } from '../shared/alertbox.component';
import { UserService } from '../services/user.service';
import { HttpService } from '../services/http.service';
import { AlertService } from '../services/alert.service';
import { SessionService } from '../services/session.service';
import { AddressComponent } from '../shared/address.component';
import { Router } from '@angular/router';

@Component({
    templateUrl: './user-profile.component.html'
})

export class UserProfileComponent implements OnInit {

    public title: string = 'User Profile';  
    public messageBox: string;
    public alerts: Array<string> = [];
    public address: Address;
    public firstName: string;
    public lastName: string;

    public firstNameInputError: Boolean;
    public lastNameInputError: Boolean;

    constructor(private userService: UserService, private sessionService: SessionService, private alertService: AlertService, private router: Router) { }

    public ngOnInit() {

        this.address = new Address();

        this.firstName = this.sessionService.firstName;
        this.lastName = this.sessionService.lastName;
        this.address.addressLine1 = this.sessionService.addressLine1;
        this.address.addressLine2 = this.sessionService.addressLine2;
        this.address.city = this.sessionService.city;
        this.address.state = this.sessionService.state;
        this.address.zipCode = this.sessionService.zipCode;            

    }

    private clearInputErrors() {
        this.firstNameInputError = false;
        this.lastNameInputError = false;     
    }


    public updateProfile(): void {

        let user = new User();
        user.firstName = this.firstName;
        user.lastName = this.lastName;
        user.addressLine1 = this.address.addressLine1;
        user.addressLine2 = this.address.addressLine2;
        user.city = this.address.city;
        user.state = this.address.state;
        user.zipCode = this.address.zipCode;

        this.clearInputErrors();

        this.userService.updateProfile(user)
            .subscribe(
            response => this.updateProfileSuccess(response),
            response => this.updateProfileOnError(response));

    }

    private updateProfileSuccess(response: User): void {

        this.alertService.renderSuccessMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();

        this.sessionService.firstName = this.firstName;
        this.sessionService.lastName = this.lastName;
        this.sessionService.addressLine1 = this.address.addressLine1;
        this.sessionService.addressLine2 = this.address.addressLine2;
        this.sessionService.city = this.address.city;
        this.sessionService.state = this.address.state;
        this.sessionService.zipCode = this.address.zipCode;           
         
    }

    private updateProfileOnError(response: User): void {
        this.alertService.renderErrorMessage(response.returnMessage);
        this.messageBox = this.alertService.returnFormattedMessage();
        this.alerts = this.alertService.returnAlerts();
        this.alertService.setValidationErrors(this, response.validationErrors);    
    }


   
}