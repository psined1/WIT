import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'validation-message',
    template: `
        <small class="text-danger">
            Name is required
        </small>
    `,
    styles: []
})
export class ValidationMessageComponent implements OnInit {

    constructor() { }

    ngOnInit() {
    }

}
