import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';


@Component({
  selector: 'app-confirm-yes-no',
  templateUrl: './confirm-yes-no.component.html',
  styleUrls: ['./confirm-yes-no.component.css']
})
export class ConfirmYesNoComponent implements OnInit {

    public result: Boolean = false;
    public title: string;
    public message: string;
    public buttonYes: string = "Yes";
    public buttonNo: string = "No";

    constructor(public bsModalRef: BsModalRef) { }

    ngOnInit() {
    }

    private confirm(): void {
        this.result = true;
        this.bsModalRef.hide();
    }

}
