import { Component, ViewChild } from '@angular/core';

import { AlertBoxComponent } from '../shared/alertbox.component';
import { UserService } from '../services/user.service';
import { HttpService } from '../services/http.service';
import { SessionService } from '../services/session.service';
import { Router } from '@angular/router';

import { User } from '../entities/user.entity';
import { TransactionInfo } from '../entities/transaction-info.entity';

@Component({
    templateUrl: './login.component.html'
})

export class LoginComponent {

    public title: string = 'Login';

    @ViewChild(AlertBoxComponent)
    private alertBox: AlertBoxComponent;

    private item: User;

    constructor(
        private userService: UserService,
        private sessionService: SessionService,
        private router: Router
    ) {
        this.item = new User();

        //////////////////////////////////////// DEBUG BEGIN ///////////////////////////////////////
        this.item.emailAddress = "psined1@gmail.com";
        this.item.password = "test";
        ////////////////////////////////////////// DEBUG END ///////////////////////////////////////
    }

    public login() : void {

        this.userService.login(this.item)
            .subscribe(
            response => this.loginOnSuccess(response),
            response => this.loginOnError(response)
            );
    }

    private loginOnSuccess(response: TransactionInfo) {

        this.sessionService.authenicated(response.data);

        this.router.navigate(['/home/home']);
    }

    private loginOnError(response: any) {

        this.alertBox.renderErrorMessage(response.returnMessage);
    }
} 
