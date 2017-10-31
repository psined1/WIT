import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal/modal-options.class';


@Component({
  selector: 'app-confirm-yes-no',
  template: `
<div class="modal-header">
    <h4 class="modal-title pull-left">{{title}}</h4>
    <button type="button" class="close pull-right" aria-label="Close" (click)="bsModalRef.hide()">
        <span aria-hidden="true">&times;</span>
    </button>
</div>
<div class="modal-body">
    {{message}}
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-primary" (click)="confirm()">{{buttonYes}}</button>
    <button type="button" class="btn btn-default" (click)="bsModalRef.hide()">{{buttonNo}}</button>
</div>
`
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
