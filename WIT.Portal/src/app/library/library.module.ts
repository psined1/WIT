import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ModalModule, TabsModule, BsModalService, BsModalRef } from 'ngx-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { LibraryService } from '../services/library.service';

import { LibraryRoutingModule } from './library-routing.module';

import { ConfirmYesNoComponent } from '../shared/confirm-yes-no/confirm-yes-no.component';

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";
import { ProductFeatureComponent } from "./product-feature/product-feature.component";

import { ProductClassListComponent } from "./product-class/product-class-list.component";
import { ProductClassComponent } from "./product-class/product-class.component";

import { CustomerListComponent } from "./customer/customer-list.component";
import { CustomerComponent } from "./customer/customer.component";


@NgModule({
    declarations: [
        ProductFeatureListComponent,
        ProductFeatureComponent,
        ProductClassListComponent,
        ProductClassComponent,
        CustomerListComponent,
        CustomerComponent
    ],
    imports: [
        CommonModule,
        LibraryRoutingModule,
        FormsModule,
        SharedModule,
        ModalModule.forRoot(),
        TabsModule.forRoot()
    ],
    entryComponents: [
        ConfirmYesNoComponent,
        ProductFeatureComponent,
        ProductClassComponent,
        CustomerComponent
    ],
    providers: [
        BsModalService,
        BsModalRef,
        LibraryService
    ]
})
export class LibraryModule { }

