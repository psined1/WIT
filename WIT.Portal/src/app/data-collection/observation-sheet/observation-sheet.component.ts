import { Component, OnInit } from '@angular/core';
import { ObservationSheetItem } from '../entities/observationSheetItem';
import { AccordionConfig } from 'ngx-bootstrap/accordion';

/*export function getAccordionConfig(): AccordionConfig {
    return Object.assign(new AccordionConfig(), { closeOthers: true });
}*/

@Component({
    selector: 'app-observation-sheet',
    templateUrl: './observation-sheet.component.html',
    styleUrls: ['./observation-sheet.component.css']
    //providers: [{ provide: AccordionConfig, useFactory: getAccordionConfig }]
})
export class ObservationSheetComponent implements OnInit {

    private title = "Observation sheet";
    private items: Array<ObservationSheetItem>;


    constructor() { }

    ngOnInit() {
        this.items = new Array<ObservationSheetItem>();

        let item: ObservationSheetItem;

        item = new ObservationSheetItem();
        item.itemID = 1;
        item.name = 'Test ' + item.itemID;
        this.items.push(item);

        item = new ObservationSheetItem();
        item.itemID = 2;
        item.name = 'Test ' + item.itemID;
        this.items.push(item);
    }

    private addItem(): void {
        let item = new ObservationSheetItem();
        item.itemID = this.items.length + 1;
        item.name = 'Test ' + item.itemID;
        this.items.push(item);
    }

    private isOpenChange(event: any): void {
        console.log(event);
    }

    private onDragEnd(event: any): void {
        console.log(event);
    }
}
