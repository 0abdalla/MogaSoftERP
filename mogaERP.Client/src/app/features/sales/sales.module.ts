import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalesRoutingModule } from './sales-routing.module';
import { QuotationsComponent } from './quotations/quotations.component';
import { ReturnsComponent } from './returns/returns.component';
import { InvoicesComponent } from './invoices/invoices.component';
import { SharedModule } from '../../shared/shared.module';
import { TableUtilityComponent } from '../../shared/table-utility/table-utility.component';


@NgModule({
  declarations: [
    QuotationsComponent,
    ReturnsComponent,
    InvoicesComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule,
    SharedModule,
    TableUtilityComponent
  ]
})
export class SalesModule { }
