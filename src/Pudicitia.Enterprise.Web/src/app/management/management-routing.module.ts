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
        path: 'identity',
        loadChildren: () =>
          import('./identity/identity.module').then(mod => mod.IdentityModule),
      },
      {
        path: 'hr',
        loadChildren: () => import('./hr/hr.module').then(mod => mod.HRModule),
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManagementRoutingModule {}
