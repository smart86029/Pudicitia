import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { HRRoutingModule } from './hr-routing.module';
import { OrganizationComponent } from './organization/organization.component';

@NgModule({
  declarations: [OrganizationComponent],
  imports: [SharedModule, HRRoutingModule],
})
export class HRModule {}
