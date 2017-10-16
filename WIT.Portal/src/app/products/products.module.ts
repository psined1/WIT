import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductInquiryComponent } from './product-inquiry.component';
import { ProductMaintenanceComponent } from './product-maintenance.component';
import { ProductsRoutingModule } from './products-routing.module';

@NgModule({
    declarations: [
        ProductInquiryComponent,
        ProductMaintenanceComponent
    ],
    imports: [
        CommonModule,
        ProductsRoutingModule
    ]

})
export class ProductsModule { }

