import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { DepartmentDialogComponent } from './department-dialog/department-dialog.component';
import { DepartmentListComponent } from './department-list/department-list.component';
import { EmployeeDialogComponent } from './employee-dialog/employee-dialog.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationComponent } from './organization.component';

@NgModule({
  declarations: [
    DepartmentDialogComponent,
    DepartmentListComponent,
    EmployeeDialogComponent,
    EmployeeListComponent,
    OrganizationComponent,
  ],
  imports: [
    SharedModule,
    OrganizationRoutingModule,
  ],
})
export class OrganizationModule { }
