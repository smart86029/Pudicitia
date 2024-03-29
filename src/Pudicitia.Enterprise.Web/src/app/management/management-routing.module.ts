import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/auth.guard';
import { ManagementComponent } from './management.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    component: ManagementComponent,
    children: [
      {
        path: 'authorization',
        loadChildren: () => import('./authorization/authorization.module').then(mod => mod.AuthorizationModule),
      },
      {
        path: 'organization',
        loadChildren: () => import('./organization/organization.module').then(mod => mod.OrganizationModule),
      },
      {
        path: 'attendance',
        loadChildren: () => import('./attendance/attendance.module').then(mod => mod.AttendanceModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManagementRoutingModule {}
