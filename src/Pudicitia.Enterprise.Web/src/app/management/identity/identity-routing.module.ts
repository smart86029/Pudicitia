import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleListComponent } from './role-list/role-list.component';

const routes: Routes = [
  {
    path: 'roles',
    component: RoleListComponent,
  },
  {
    path: 'permissions',
    component: PermissionListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityRoutingModule {}
