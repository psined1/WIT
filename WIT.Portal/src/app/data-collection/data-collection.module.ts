﻿import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

import { AccordionModule } from 'ngx-bootstrap/accordion';
import { ModalModule } from 'ngx-bootstrap';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DragulaModule } from 'ng2-dragula';

import { DataCollectionRoutingModule } from './data-collection-routing.module';
//import { CustomerService } from '../services/customer.service';

import { ObservationSheetComponent } from './observation-sheet/observation-sheet.component';
import { ConfirmYesNoComponent } from '../shared/confirm-yes-no/confirm-yes-no.component';
import { StepMaintenanceComponent } from './observation-sheet/step-maintenance.component';

@NgModule({
    declarations: [
        ObservationSheetComponent,
        StepMaintenanceComponent
    ],
    imports: [
        CommonModule,
        DataCollectionRoutingModule,
        FormsModule,
        SharedModule,
        ModalModule.forRoot(),
        AccordionModule.forRoot(),
        DragulaModule
    ],
    entryComponents: [
        ConfirmYesNoComponent,
        StepMaintenanceComponent
    ],
    providers: [
        BsModalService,
        BsModalRef
    ]
})
export class DataCollectionModule { }