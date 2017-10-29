import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ModalModule, TabsModule, BsModalService, BsModalRef } from 'ngx-bootstrap';

import { SharedModule } from '../shared/shared.module';
import { LibraryService } from '../services/library.service';

import { LibraryRoutingModule } from './library-routing.module';

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";
import { ConfirmYesNoComponent } from '../shared/confirm-yes-no/confirm-yes-no.component';


@NgModule({
    declarations: [
        ProductFeatureListComponent
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
        ConfirmYesNoComponent
    ],
    providers: [
        BsModalService,
        BsModalRef,
        LibraryService
    ]
})
export class LibraryModule { }

