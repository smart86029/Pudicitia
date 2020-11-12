import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { IdentityRoutingModule } from './identity-routing.module';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';
import { PermissionDetailComponent } from './permission-detail/permission-detail.component';

@NgModule({
  declarations: [
    PermissionListComponent,
    RoleDetailComponent,
    RoleListComponent,
    PermissionDetailComponent,
  ],
  imports: [SharedModule, IdentityRoutingModule],
})
export class IdentityModule {}
