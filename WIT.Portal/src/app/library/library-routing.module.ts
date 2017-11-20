import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizationGuard } from "../authorization-guard";

import { ProductFeatureListComponent } from "./product-feature/product-feature-list.component";
import { ProductClassListComponent } from "./product-class/product-class-list.component";
import { ProductListComponent } from "./product/product-list.component";
import { CustomerListComponent } from "./customer/customer-list.component";
import { ItemListComponent } from "./item/item-list.component";

const libraryRoutes: Routes = [
    { path: 'product-features', component: ProductFeatureListComponent, canActivate: [AuthorizationGuard] },
    { path: 'product-classes', component: ProductClassListComponent, canActivate: [AuthorizationGuard] },
    { path: 'products', component: ProductListComponent, canActivate: [AuthorizationGuard] },
    { path: 'customers', component: CustomerListComponent, canActivate: [AuthorizationGuard] },
    { path: 'item', component: ItemListComponent, canActivate: [AuthorizationGuard] },
    { path: '', component: ProductFeatureListComponent }
]

@NgModule({
    imports: [
        RouterModule.forChild(libraryRoutes)
    ],
    exports: [RouterModule]
})
export class LibraryRoutingModule { }
