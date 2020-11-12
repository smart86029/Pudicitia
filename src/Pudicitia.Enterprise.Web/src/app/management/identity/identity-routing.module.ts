import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';

const routes: Routes = [
  {
    path: 'roles',
    component: RoleListComponent,
  },
  {
    path: 'roles/new',
    component: RoleDetailComponent,
  },
  {
    path: 'roles/:id',
    component: RoleDetailComponent,
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
export class IdentityRoutingModule { }
