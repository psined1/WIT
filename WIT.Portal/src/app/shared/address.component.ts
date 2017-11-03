import { Component, OnInit, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { Address } from '../entities/address.entity'

@Component({
    selector: 'address-form',
    templateUrl: './address.component.html'
})

export class AddressComponent implements OnInit {

    @Input()
    public validationErrors: any = {};

    @Input()
    @Output()
    public address: Address;

    constructor() { }

    public ngOnInit() {

    }

    public ngOnChanges(changes: SimpleChanges) {
        for (let propName in changes) {
            let chng = changes[propName];
            let cur = JSON.stringify(chng.currentValue);
            let prev = JSON.stringify(chng.previousValue);
            console.log(`${propName}: currentValue = ${cur}, previousValue = ${prev}`);
        }
    }
}