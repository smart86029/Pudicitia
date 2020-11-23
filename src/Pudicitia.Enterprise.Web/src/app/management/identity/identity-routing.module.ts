import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionDetailComponent } from './permission-detail/permission-detail.component';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {
    path: 'users',
    component: UserListComponent,
  },
  {
    path: 'users/new',
    component: UserFormComponent,
  },
  {
    path: 'users/:id',
    component: UserFormComponent,
  },
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
  {
    path: 'permissions/new',
    component: PermissionDetailComponent,
  },
  {
    path: 'permissions/:id',
    component: PermissionDetailComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityRoutingModule { }
