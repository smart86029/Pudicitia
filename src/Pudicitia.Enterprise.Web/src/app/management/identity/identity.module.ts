import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { IdentityRoutingModule } from './identity-routing.module';
import { RoleListComponent } from './role-list/role-list.component';

@NgModule({
  declarations: [RoleListComponent],
  imports: [SharedModule, IdentityRoutingModule],
})
export class IdentityModule {}
