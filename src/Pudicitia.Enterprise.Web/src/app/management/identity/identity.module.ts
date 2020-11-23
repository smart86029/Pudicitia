import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { IdentityRoutingModule } from './identity-routing.module';
import { PermissionDetailComponent } from './permission-detail/permission-detail.component';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';
import { UserFormComponent } from './user-form/user-form.component';
import { UserListComponent } from './user-list/user-list.component';

@NgModule({
  declarations: [
    PermissionDetailComponent,
    PermissionListComponent,
    RoleDetailComponent,
    RoleListComponent,
    UserFormComponent,
    UserListComponent,
  ],
  imports: [SharedModule, IdentityRoutingModule],
})
export class IdentityModule { }
