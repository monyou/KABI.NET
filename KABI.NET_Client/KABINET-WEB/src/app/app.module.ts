import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialsModule } from './angular-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AccessDeniedComponent } from './@shared/pages/access-denied/access-denied.component';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NavbarComponent } from './@shared/layout/navbar/navbar.component';
import { LoginComponent } from './@modules/auth/pages/login/login.component';
import { LaundryComponent } from './@modules/laundry/laundry.component';
import { ChangePasswordComponent } from './@modules/settings/pages/change-password/change-password.component';
import { AdminDashboardComponent } from './@modules/admin/pages/admin-dashboard/admin-dashboard.component';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { CustomMatPaginatorIntl } from './@shared/helpers/mat-paginator.extension';
import { createTranslateLoader } from './@shared/helpers/functions';
import { AngularFireModule } from '@angular/fire';
import { TavernSchedulerComponent } from './@modules/tavern/pages/tavern-scheduler/tavern-scheduler.component';
import { AccessTokenInterceptorService } from './@shared/interceptors/access-token.interceptor';
import { OwlDateTimeModule, OwlNativeDateTimeModule, OWL_DATE_TIME_LOCALE, OwlDateTimeIntl } from 'ng-pick-datetime';
import { BGIntl } from './@shared/helpers/global-consts';
import { TavernComponent } from './@modules/tavern/pages/tavern/tavern.component';

@NgModule({
  declarations: [
    AppComponent,
    AccessDeniedComponent,
    NavbarComponent,
    LoginComponent,
    LaundryComponent,
    ChangePasswordComponent,
    AdminDashboardComponent,
    TavernSchedulerComponent,
    TavernComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient]
      }
    }),
    AngularMaterialsModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AccessTokenInterceptorService, multi: true },
    {
      provide: MatPaginatorIntl,
      useFactory: (translate) => {
        const service = new CustomMatPaginatorIntl();
        service.injectTranslateService(translate);
        return service;
      },
      deps: [TranslateService]
    },
    { provide: OWL_DATE_TIME_LOCALE, useValue: 'bg' },
    { provide: OwlDateTimeIntl, useClass: BGIntl }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
