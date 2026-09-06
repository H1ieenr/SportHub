// angular import
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

// Project import
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { GuestLayoutComponent } from './layouts/guest-layout/guest-layout.component';

const routes: Routes = [
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
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'brand',
        loadComponent: () => import('./features/catalog/brand/brand-list/brand-list.component').then((c) => c.BrandListComponent)
      },
      // {
      //   path: 'color',
      //   loadComponent: () => import('./demo/component/basic-component/color/color.component').then((c) => c.ColorComponent)
      // },
      // {
      //   path: 'sample-page',
      //   loadComponent: () => import('./demo/others/sample-page/sample-page.component').then((c) => c.SamplePageComponent)
      // }
    ]
  },
  { path: '**', redirectTo: '' },
  {
    path: '',
    component: GuestLayoutComponent,
    children: [
      {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login.component').then((c) => c.LoginComponent)
      },
      // {
      //   path: 'register',
      //   loadComponent: () =>
      //     import('./demo/pages/authentication/auth-register/auth-register.component').then((c) => c.AuthRegisterComponent)
      // }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
