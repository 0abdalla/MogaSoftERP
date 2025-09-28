import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdditionNotificationsComponent } from './bank-actions/addition-notifications/addition-notifications.component';
import { DebitNotificationsComponent } from './bank-actions/debit-notifications/debit-notifications.component';

const routes: Routes = [
  {path:"bank-actions/addition-notifications" , component : AdditionNotificationsComponent},
  {path:"bank-actions/debit-notifications" , component : DebitNotificationsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinTreeRoutingModule { }
