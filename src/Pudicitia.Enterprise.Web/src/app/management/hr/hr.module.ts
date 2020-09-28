import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { DepartmentDialogComponent } from './department-dialog/department-dialog.component';
import { HRRoutingModule } from './hr-routing.module';
import { OrganizationComponent } from './organization/organization.component';

@NgModule({
  declarations: [OrganizationComponent, DepartmentDialogComponent],
  imports: [SharedModule, HRRoutingModule],
})
export class HRModule {}
