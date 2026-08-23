// <reference types="@angular/localize" />
import { enableProdMode, importProvidersFrom } from '@angular/core';
import { bootstrapApplication, BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { environment } from './environments/environment';
import { AppRoutingModule } from './app/app-routing.module';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/core/auth/auth.interceptor';

if (environment.production) {
  enableProdMode();
}

// bootstrapApplication(App, appConfig).catch((err) => console.error(err));
bootstrapApplication(AppComponent, {
  providers: [importProvidersFrom(BrowserModule, AppRoutingModule),
              provideHttpClient(withInterceptors([authInterceptor]))]
}).catch((err) => console.error(err));
