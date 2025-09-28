import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinTreeRoutingModule } from './fin-tree-routing.module';
import { AdditionNotificationsComponent } from './bank-actions/addition-notifications/addition-notifications.component';
import { DebitNotificationsComponent } from './bank-actions/debit-notifications/debit-notifications.component';
import { SharedModule } from '../../shared/shared.module';


@NgModule({
  declarations: [
    AdditionNotificationsComponent,
    DebitNotificationsComponent
  ],
  imports: [
    CommonModule,
    FinTreeRoutingModule,
    SharedModule
  ]
})
export class FinTreeModule { }
