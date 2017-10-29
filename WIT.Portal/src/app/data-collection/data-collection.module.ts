import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';

//BS imports
import { ModalModule, TabsModule, AccordionModule, BsModalService, BsModalRef } from 'ngx-bootstrap';
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
        TabsModule.forRoot(),
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
