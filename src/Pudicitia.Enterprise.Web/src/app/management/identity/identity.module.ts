import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { IdentityRoutingModule } from './identity-routing.module';
import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleListComponent } from './role-list/role-list.component';

@NgModule({
  declarations: [PermissionListComponent, RoleListComponent],
  imports: [SharedModule, IdentityRoutingModule],
})
export class IdentityModule {}
