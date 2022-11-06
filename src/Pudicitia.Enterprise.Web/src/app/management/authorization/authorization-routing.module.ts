import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthorizationComponent } from './authorization.component';
import { PermissionFormComponent } from './permission-form/permission-form.component';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleFormComponent } from './role-form/role-form.component';
import { RoleListComponent } from './role-list/role-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'users',
  },
  {
    path: '',
    component: AuthorizationComponent,
    children: [
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
        component: RoleFormComponent,
      },
      {
        path: 'roles/:id',
        component: RoleFormComponent,
      },
      {
        path: 'permissions',
        component: PermissionListComponent,
      },
      {
        path: 'permissions/new',
        component: PermissionFormComponent,
      },
      {
        path: 'permissions/:id',
        component: PermissionFormComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthorizationRoutingModule {}
