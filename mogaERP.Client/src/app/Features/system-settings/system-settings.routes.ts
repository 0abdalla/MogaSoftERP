import { Routes } from '@angular/router';

export const SystemSettingsRoutes: Routes = [
    {
        path: 'system-settings',
        children: [
            {
                path: 'customers',
                loadComponent: () => import('./customers/customers.component').then(m => m.CustomersComponent),
            },
            {
                path: 'providers',
                loadComponent: () => import('./providers/providers.component').then(m => m.ProvidersComponent),
            },
            {
                path: 'stores-settings',
                children: [
                    {
                        path: 'main-groups',
                        loadComponent: () => import('./stores-settings/main-groups/main-groups.component').then(m => m.MainGroupsComponent),
                    },
                    {
                        path: 'items-groups',
                        loadComponent: () => import('./stores-settings/items-groups/items-groups.component').then(m => m.ItemsGroupsComponent),
                    },
                    {
                        path: 'item-units',
                        loadComponent: () => import('./stores-settings/item-units/item-units.component').then(m => m.ItemUnitsComponent),
                    },
                    {
                        path: 'items',
                        loadComponent: () => import('./stores-settings/items/items.component').then(m => m.ItemsComponent),
                    },
                    {
                        path: 'stores',
                        loadComponent: () => import('./stores-settings/stores/stores.component').then(m => m.StoresComponent),
                    },
                    {
                        path: 'stores-types',
                        loadComponent: () => import('./stores-settings/stores-types/stores-types.component').then(m => m.StoresTypesComponent),
                    },
                    { path: '', redirectTo: 'main-groups', pathMatch: 'full' }
                ]
            },
            {
                path: 'finance-settings',
                children: [
                    {
                        path: 'financial-tree',
                        loadComponent: () => import('./finance-settings/fin-tree/fin-tree.component').then(m => m.FinTreeComponent),
                    },
                    {
                        path: 'accounting-guide',
                        loadComponent: () => import('./finance-settings/accounting-guide/accounting-guide.component').then(m => m.AccountingGuideComponent),
                    },
                    {
                        path: 'treasuries',
                        loadComponent: () => import('./finance-settings/treasuries/treasuries.component').then(m => m.TreasuriesComponent),
                    },
                    {
                        path: 'banks',
                        loadComponent: () => import('./finance-settings/banks/banks.component').then(m => m.BanksComponent),
                    },
                    {
                        path: 'financial-year',
                        loadComponent: () => import('./finance-settings/fin-year/fin-year.component').then(m => m.FinYearComponent),
                    },
                    {
                        path: 'cost-center',
                        loadComponent: () => import('./finance-settings/cost-center/cost-center.component').then(m => m.CostCenterComponent),
                    },
                    { path: '', redirectTo: 'financial-tree', pathMatch: 'full' }
                ]
            },
            { path: '', redirectTo: 'customers', pathMatch: 'full' }
        ]
    }
];
