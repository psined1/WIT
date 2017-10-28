import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerInquiryComponent } from './customer-inquiry.component';
import { CustomerMaintenanceComponent } from './customer-maintenance.component';
import { CustomersRoutingModule } from './customers-routing.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

import { ModalModule } from 'ngx-bootstrap';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CustomerService } from '../services/customer.service';

@NgModule({
    declarations: [
        CustomerInquiryComponent,
        CustomerMaintenanceComponent
    ],
    imports: [
        CommonModule,
        CustomersRoutingModule,
        FormsModule,
        SharedModule,
        ModalModule.forRoot()
    ],
    entryComponents: [CustomerMaintenanceComponent],
    providers: [CustomerService, BsModalService, BsModalRef]
})
export class CustomersModule { }
