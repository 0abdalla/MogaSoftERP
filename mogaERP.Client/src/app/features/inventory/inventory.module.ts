import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InventoryRoutingModule } from './inventory-routing.module';
import { ReciptPermissionsComponent } from './recipt-permissions/recipt-permissions.component';
import { SharedModule } from '../../shared/shared.module';
import { ExchangeRequestComponent } from './exchange-request/exchange-request.component';
import { ExchangePermissionsComponent } from './exchange-permissions/exchange-permissions.component';


@NgModule({
  declarations: [
    ReciptPermissionsComponent,
    ExchangeRequestComponent,
    ExchangePermissionsComponent
  ],
  imports: [
    CommonModule,
    InventoryRoutingModule,
    SharedModule
  ]
})
export class InventoryModule { }
