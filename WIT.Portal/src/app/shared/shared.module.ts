import { AlertModule } from 'ngx-bootstrap';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { AlertBoxComponent } from '../shared/alertbox.component';
import { AddressComponent } from '../shared/address.component';

import { DataGrid } from '../shared/datagrid/datagrid.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConfirmYesNoComponent } from './confirm-yes-no/confirm-yes-no.component';

import { ModalModule } from 'ngx-bootstrap';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


@NgModule({
    declarations: [     
        AlertBoxComponent,
        AddressComponent,
        DataGrid,
        ConfirmYesNoComponent
    ],   
    imports: [
        FormsModule,      
        AlertModule.forRoot(), 
        CommonModule,
        ModalModule.forRoot()
    ],
    exports: [
        AlertBoxComponent,
        AddressComponent,
        DataGrid
    ],
    entryComponents: [ConfirmYesNoComponent],
    providers: [AlertBoxComponent, BsModalService, BsModalRef],
    schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
})
export class SharedModule { }
