import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemSettingsRoutingModule } from './system-settings-routing.module';
import { CustomersComponent } from './customers/customers.component';
import { ProvidersComponent } from './providers/providers.component';
import { ItemsGroupsComponent } from './stores-settings/items-groups/items-groups.component';
import { MainGroupsComponent } from './stores-settings/main-groups/main-groups.component';
import { ItemsComponent } from './stores-settings/items/items.component';
import { ItemUnitsComponent } from './stores-settings/item-units/item-units.component';
import { StoresComponent } from './stores-settings/stores/stores.component';
import { StoresTypesComponent } from './stores-settings/stores-types/stores-types.component';
import { FinTreeComponent } from './finance-settings/fin-tree/fin-tree.component';
import { AccountingGuideComponent } from './finance-settings/accounting-guide/accounting-guide.component';
import { TreasuriesComponent } from './finance-settings/treasuries/treasuries.component';
import { BanksComponent } from './finance-settings/banks/banks.component';
import { FinYearComponent } from './finance-settings/fin-year/fin-year.component';
import { CostCenterComponent } from './finance-settings/cost-center/cost-center.component';
import { SharedModule } from '../../shared/shared.module';
import { ToastModule } from 'primeng/toast';

@NgModule({
  declarations: [
    CustomersComponent,
    ProvidersComponent,
    ItemsGroupsComponent,
    MainGroupsComponent,
    ItemsComponent,
    ItemUnitsComponent,
    StoresComponent,
    StoresTypesComponent,
    FinTreeComponent,
    AccountingGuideComponent,
    TreasuriesComponent,
    BanksComponent,
    FinYearComponent,
    CostCenterComponent
  ],
  imports: [
    CommonModule,
    SystemSettingsRoutingModule,
    SharedModule,
    ToastModule
  ]
})
export class SystemSettingsModule { }
