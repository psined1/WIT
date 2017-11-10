import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class BlockUIService {

    public blockUIEvent: EventEmitter<{ value: Boolean }>;

    constructor() {
        this.blockUIEvent = new EventEmitter();
    }

    public startBlock() {
        //console.log('startBlock');
        this.blockUIEvent.emit({
            value: true
        });
    }

    public stopBlock() {
        //console.log('stopBlock');
        this.blockUIEvent.emit({
            value: false
        });
    }
}