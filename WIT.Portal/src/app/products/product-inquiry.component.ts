import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-product-inquiry',
    templateUrl: './product-inquiry.component.html',
    styleUrls: ['./product-inquiry.component.css']
})
export class ProductInquiryComponent implements OnInit {

    public title: string;
    constructor() { }

    ngOnInit() {
        this.title = "Product Inquiry";
    }

}
