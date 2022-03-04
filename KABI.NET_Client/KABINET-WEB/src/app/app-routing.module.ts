import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './@modules/auth/guards/auth.guard';
import { LoginAvaliableGuard } from './@modules/auth/guards/login-avaliable.guard';
import { AccessDeniedComponent } from './@shared/pages/access-denied/access-denied.component';
import { NavbarComponent } from './@shared/layout/navbar/navbar.component';
import { LoginComponent } from './@modules/auth/pages/login/login.component';
import { LaundryComponent } from './@modules/laundry/laundry.component';
import { ChangePasswordComponent } from './@modules/settings/pages/change-password/change-password.component';
import { AdminDashboardComponent } from './@modules/admin/pages/admin-dashboard/admin-dashboard.component';
import { AdminGuard } from './@modules/admin/guards/admin.guard';
import { TavernSchedulerComponent } from './@modules/tavern/pages/tavern-scheduler/tavern-scheduler.component';
import { TavernComponent } from './@modules/tavern/pages/tavern/tavern.component';

const routes: Routes = [
  {
    path: '', component: NavbarComponent, children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: '/login'
      },
      {
        path: 'login',
        component: LoginComponent,
        canActivate: [LoginAvaliableGuard]
      },
      {
        path: 'admin-dashboard',
        component: AdminDashboardComponent,
        canActivate: [AuthGuard, AdminGuard]
      },
      {
        path: 'laundry',
        component: LaundryComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'tavern',
        component: TavernComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'tavern-scheduler',
        component: TavernSchedulerComponent,
        canActivate: [AuthGuard, AdminGuard]
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
        canActivate: [AuthGuard]
      },
      {
        path: 'access-denied',
        component: AccessDeniedComponent,
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
