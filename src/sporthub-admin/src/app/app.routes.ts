import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    //   {
    //     path: 'dashboard',
    //     loadComponent: () =>
    //       import('./features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
    //   },
      // brands, categories, products sẽ thêm khi làm feature
    ],
  },
];