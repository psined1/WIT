import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizationGuard } from "../authorization-guard";

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";
import { CustomerListComponent } from "./customer/customer-list.component";

const libraryRoutes: Routes = [
    { path: 'product-features', component: ProductFeatureListComponent, canActivate: [AuthorizationGuard] },
    { path: 'customers', component: CustomerListComponent, canActivate: [AuthorizationGuard] },
    { path: '', component: ProductFeatureListComponent }
]

@NgModule({
    imports: [
        RouterModule.forChild(libraryRoutes)
    ],
    exports: [RouterModule]
})
export class LibraryRoutingModule { }
