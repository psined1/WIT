
import { Injectable } from '@angular/core';

@Injectable()
export class AlertService {

    private alerts = [];
    private messageBox = "";

    constructor() { }

    setValidationErrors(scope, validationErrors) {       
        for (var prop in validationErrors) {
            var property = prop + "InputError";
            scope[property] = true;
        }
    }

    returnFormattedMessage() {
        return this.messageBox;
    }

    returnAlerts() {
        return this.alerts;
    }

    renderErrorMessage(message): [string, any[]] {

        let messageBox = this.formatMessage(message);   
        this.alerts = [];
        this.messageBox = messageBox;
        this.alerts.push({ msg: messageBox, type: 'danger', closable: true });

        return [this.messageBox, this.alerts];
    };

    renderSuccessMessage(message) : [string, any[]] {

        let messageBox = this.formatMessage(message);

        this.alerts = [];
        this.messageBox = messageBox;
        this.alerts.push({ msg: messageBox, type: 'success', closable: true });

        return [this.messageBox, this.alerts];
    };

    renderWarningMessage(message): [string, any[]] {

        let messageBox = this.formatMessage(message);

        this.alerts = [];
        this.messageBox = messageBox;
        this.alerts.push({ msg: messageBox, type: 'warning', closable: true });

        return [this.messageBox, this.alerts];
    };

    renderInformationalMessage(message): [string, any[]] {

        let messageBox = this.formatMessage(message);

        this.alerts = [];
        this.messageBox = messageBox;
        this.alerts.push({ msg: messageBox, type: 'info', closable: true });

        return [this.messageBox, this.alerts];
    };

    formatMessage(message) {

        let messageBox = "";

        if (Array.isArray(message) == true ) {      
            for (var i = 0; i < message.length; i++) {
                messageBox = messageBox + message[i] + "<br/>";
            }
        }
        else {
            messageBox = message;
        }

        return messageBox;

    }

}
