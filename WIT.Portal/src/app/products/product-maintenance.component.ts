import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-product-maintenance',
    templateUrl: './product-maintenance.component.html',
    styleUrls: ['./product-maintenance.component.css']
})
export class ProductMaintenanceComponent implements OnInit {

    public title: string;
    constructor() { }

    ngOnInit() {
        this.title = "Product Maintenance";
    }

}
