import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizationGuard } from "../authorization-guard";

import { ObservationSheetComponent } from './observation-sheet/observation-sheet.component';

const dataCollectionRoutes: Routes = [
    { path: '', component: ObservationSheetComponent, canActivate: [AuthorizationGuard] },
    { path: 'sheet', component: ObservationSheetComponent, canActivate: [AuthorizationGuard]  }
]

@NgModule({
    imports: [
        RouterModule.forChild(dataCollectionRoutes)
    ],
    exports: [RouterModule]
})
export class DataCollectionRoutingModule {}
