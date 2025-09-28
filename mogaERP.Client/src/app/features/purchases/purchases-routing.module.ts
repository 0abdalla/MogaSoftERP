import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseOrderComponent } from './purchase-order/purchase-order.component';
import { PriceQoutationsComponent } from './price-qoutations/price-qoutations.component';
import { PurchaseRequestComponent } from './purchase-request/purchase-request.component';

const routes: Routes = [
  
  { path: 'purchase-order', component: PurchaseOrderComponent },
  { path: 'price-quotations', component: PriceQoutationsComponent },
  { path: 'purchase-request', component: PurchaseRequestComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchasesRoutingModule { }
