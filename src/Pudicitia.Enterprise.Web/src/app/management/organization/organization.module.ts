import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { DepartmentFormComponent } from './department-form/department-form.component';
import { DepartmentListComponent } from './department-list/department-list.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { JobFormComponent } from './job-form/job-form.component';
import { JobListComponent } from './job-list/job-list.component';
import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationComponent } from './organization.component';

@NgModule({
  declarations: [
    DepartmentFormComponent,
    DepartmentListComponent,
    EmployeeFormComponent,
    EmployeeListComponent,
    JobListComponent,
    JobFormComponent,
    OrganizationComponent,
  ],
  imports: [SharedModule, OrganizationRoutingModule],
})
export class OrganizationModule {}
