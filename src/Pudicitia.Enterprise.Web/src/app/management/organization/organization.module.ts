import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { DepartmentDialogComponent } from './department-dialog/department-dialog.component';
import { EmployeeDialogComponent } from './employee-dialog/employee-dialog.component';
import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationComponent } from './organization.component';

@NgModule({
  declarations: [
    DepartmentDialogComponent,
    EmployeeDialogComponent,
    OrganizationComponent,
  ],
  imports: [
    SharedModule,
    OrganizationRoutingModule,
  ],
})
export class OrganizationModule { }
