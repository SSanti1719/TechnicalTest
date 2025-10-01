import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { CustomerOrdersComponent } from './features/customers/customer-orders/customer-orders.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'customers', component: CustomerOrdersComponent },
      { path: '', redirectTo: 'customers', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'customers' }
];
