import { Routes } from '@angular/router';
import { PurchasesRoutes } from './Features/purchases/purchases.routes';
import { SystemSettingsRoutes } from './Features/system-settings/system-settings.routes';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./Auth/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'dashboard',
        loadComponent: () => import('./Features/dashboard/dashboard.component').then(m => m.DashboardComponent)
    },
    ...PurchasesRoutes,
    ...SystemSettingsRoutes,
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: '**', redirectTo: '/dashboard' }
];
