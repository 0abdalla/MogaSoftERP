import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchasesRoutingModule } from './purchases-routing.module';
import { PurchaseRequestComponent } from './purchase-request/purchase-request.component';
import { PurchaseOrderComponent } from './purchase-order/purchase-order.component';
import { PriceQoutationsComponent } from './price-qoutations/price-qoutations.component';
import { SharedModule } from '../../shared/shared.module';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';


@NgModule({
  declarations: [
    PurchaseRequestComponent,
    PurchaseOrderComponent,
    PriceQoutationsComponent
  ],
  imports: [
    CommonModule,
    PurchasesRoutingModule,
    SharedModule,
    FloatLabelModule,
    InputTextModule
  ]
})
export class PurchasesModule { }
