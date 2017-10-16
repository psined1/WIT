import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductMaintenanceComponent } from './product-maintenance.component';
import { ProductInquiryComponent } from './product-inquiry.component';
import { AuthorizationGuard } from "../authorization-guard";

const productsRoutes: Routes = [
    { path: '', component: ProductInquiryComponent },
    { path: 'product-inquiry', component: ProductInquiryComponent, canActivate: [AuthorizationGuard]  },
    { path: 'product-maintenance', component: ProductMaintenanceComponent, canActivate: [AuthorizationGuard]  }
]

@NgModule({
    imports: [
        RouterModule.forChild(productsRoutes)
    ],
    exports: [RouterModule]
})
export class ProductsRoutingModule { }
