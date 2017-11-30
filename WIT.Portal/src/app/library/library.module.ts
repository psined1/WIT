import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ModalModule, TabsModule, AccordionModule, BsModalService, BsModalRef, TypeaheadModule } from 'ngx-bootstrap';
import { DragulaModule } from 'ng2-dragula';

import { SharedModule } from '../shared/shared.module';
import { LibraryService } from '../services/library.service';

import { LibraryRoutingModule } from './library-routing.module';

import { ConfirmYesNoComponent } from '../shared/confirm-yes-no/confirm-yes-no.component';

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";
import { ProductFeatureComponent } from "./product-feature/product-feature.component";
import { ProductClassListComponent } from "./product-class/product-class-list.component";
import { ProductClassComponent } from "./product-class/product-class.component";
import { ProductListComponent } from "./product/product-list.component";
import { ProductComponent } from "./product/product.component";
import { CustomerListComponent } from "./customer/customer-list.component";
import { CustomerComponent } from "./customer/customer.component";

import { ItemListComponent } from './item/item-list.component';
import { ItemComponent } from './item/item.component';
import { ItemTypeListComponent } from './item-type/item-type-list.component';
import { ItemTypeComponent } from './item-type/item-type.component';


@NgModule({
    declarations: [
        ProductFeatureListComponent, ProductFeatureComponent,
        ProductClassListComponent, ProductClassComponent,
        ProductListComponent, ProductComponent,
        CustomerListComponent, CustomerComponent,

        ItemListComponent, ItemComponent,
        ItemTypeListComponent, ItemTypeComponent
    ],
    imports: [
        CommonModule,
        LibraryRoutingModule,
        FormsModule,
        SharedModule,
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        TypeaheadModule.forRoot(),
        AccordionModule.forRoot(),
        DragulaModule
    ],
    entryComponents: [
        ConfirmYesNoComponent,
        ProductFeatureComponent,
        ProductClassComponent,
        ProductComponent,
        CustomerComponent,
        ItemComponent,
        ItemTypeComponent
    ],
    providers: [
        BsModalService,
        BsModalRef,
        LibraryService
    ]
})
export class LibraryModule { }

