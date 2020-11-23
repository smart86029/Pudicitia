import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { IdentityRoutingModule } from './identity-routing.module';
import { PermissionFormComponent } from './permission-form/permission-form.component';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleFormComponent } from './role-form/role-form.component';
import { RoleListComponent } from './role-list/role-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserListComponent } from './user-list/user-list.component';

@NgModule({
  declarations: [
    PermissionFormComponent,
    PermissionListComponent,
    RoleFormComponent,
    RoleListComponent,
    UserFormComponent,
    UserListComponent,
  ],
  imports: [SharedModule, IdentityRoutingModule],
})
export class IdentityModule { }
