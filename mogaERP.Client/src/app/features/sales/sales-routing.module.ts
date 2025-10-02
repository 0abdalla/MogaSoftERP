import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { QuotationsComponent } from './quotations/quotations.component';
import { ReturnsComponent } from './returns/returns.component';
import { InvoicesComponent } from './invoices/invoices.component';

const routes: Routes = [
  {
    path: 'quotations',
    component: QuotationsComponent
  },
  {
    path: 'returns',
    component: ReturnsComponent
  },
  {
    path: 'invoices',
    component: InvoicesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule { }
