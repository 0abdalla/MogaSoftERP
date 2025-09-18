import { Routes } from '@angular/router';

export const PurchasesRoutes: Routes = [
    {
        path: 'purchases',
        children: [
            {
                path: 'purchase-order',
                loadComponent: () => import('./purchase-order/purchase-order.component').then((m) => m.PurchaseOrderComponent),
            },
            {
                path: 'price-quotations',
                loadComponent: () => import('./price-quotations/price-quotations.component').then((m) => m.PriceQuotationsComponent),
            },
            {
                path: 'purchase-request', 
                loadComponent: () => import('./purchase-request/purchase-request.component').then((m) => m.PurchaseRequestComponent),
            },
            {
                path: '',
                redirectTo: 'purchase-order',
                pathMatch: 'full',
            },
        ],
    },
];
