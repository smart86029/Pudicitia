import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { DepartmentDialogComponent } from './department-dialog/department-dialog.component';
import { EmployeeDialogComponent } from './employee-dialog/employee-dialog.component';
import { HRRoutingModule } from './hr-routing.module';
import { OrganizationComponent } from './organization/organization.component';

@NgModule({
  declarations: [
    DepartmentDialogComponent,
    EmployeeDialogComponent,
    OrganizationComponent,
  ],
  imports: [SharedModule, HRRoutingModule],
})
export class HRModule {}
