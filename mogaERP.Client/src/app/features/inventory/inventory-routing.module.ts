import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReciptPermissionsComponent } from './recipt-permissions/recipt-permissions.component';
import { ExchangeRequestComponent } from './exchange-request/exchange-request.component';
import { ExchangePermissionsComponent } from './exchange-permissions/exchange-permissions.component';

const routes: Routes = [
  {
    path: 'recipt-permissions',
    component: ReciptPermissionsComponent
  },
  {
    path: 'exchange-request',
    component: ExchangeRequestComponent
  },
  {
    path: 'exchange-permissions',
    component: ExchangePermissionsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }
