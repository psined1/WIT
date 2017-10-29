import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizationGuard } from "../authorization-guard";

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";

const libraryRoutes: Routes = [
    { path: '', component: ProductFeatureListComponent },
    { path: 'product-features', component: ProductFeatureListComponent, canActivate: [AuthorizationGuard]  }
    //{ path: 'product-maintenance', component: ProductMaintenanceComponent, canActivate: [AuthorizationGuard]  }
]

@NgModule({
    imports: [
        RouterModule.forChild(libraryRoutes)
    ],
    exports: [RouterModule]
})
export class LibraryRoutingModule { }
