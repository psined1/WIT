import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'alertbox',
    templateUrl: './alertbox.component.html',
    styleUrls: ['./alertbox.component.css']
})

export class AlertBoxComponent implements OnInit {

    @Input()
    public alerts: {
        msg: string,
        type: string,
        closable: Boolean
    }[];

    @Input() public messageBox: string;
    @Input() public delay: number;

    public showSpinner: Boolean = false;

    constructor() { }

    public ngOnInit() {

    }

    public closeAlert(i: number): void {
        this.alerts.splice(i, 1);
    }

    public clear(): void {
        this.alerts = [];
        this.messageBox = "";
    }

    public closeAlertBox(): void {
        setTimeout(() => {
            this.alerts.splice(0, 1);
        }, this.delay)
    }

    public startSpinner(): void {
        //  this.showSpinner = true;
    }

    public renderErrorMessage(message): void {
        this.renderMessage(message, 'danger');
    };

    public renderSuccessMessage(message): void {
        this.renderMessage(message, 'success');
    };

    public renderWarningMessage(message): void {
        this.renderMessage(message, 'warning');
    };

    public renderInformationalMessage(message): void {
        this.renderMessage(message, 'info');
    };

    private renderMessage(message: any, type: string): void {

        let messageBox = this.formatMessage(message);

        this.alerts = [];
        this.messageBox = messageBox;
        this.alerts.push({ msg: messageBox, type: type, closable: true });
    };

    private formatMessage(message):string {

        let messageBox = "";

        if (Array.isArray(message) == true) {
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
