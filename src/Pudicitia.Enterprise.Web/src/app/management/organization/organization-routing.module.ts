import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DepartmentFormComponent } from './department-form/department-form.component';
import { DepartmentListComponent } from './department-list/department-list.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { JobFormComponent } from './job-form/job-form.component';
import { JobListComponent } from './job-list/job-list.component';
import { OrganizationComponent } from './organization.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'departments',
  },
  {
    path: '',
    component: OrganizationComponent,
    children: [
      {
        path: 'departments',
        component: DepartmentListComponent,
      },
      {
        path: 'departments/new',
        component: DepartmentFormComponent,
      },
      {
        path: 'departments/:id',
        component: DepartmentFormComponent,
      },
      {
        path: 'employees',
        component: EmployeeListComponent,
      },
      {
        path: 'employees/new',
        component: EmployeeFormComponent,
      },
      {
        path: 'employees/:id',
        component: EmployeeFormComponent,
      },
      {
        path: 'jobs',
        component: JobListComponent,
      },
      {
        path: 'jobs/new',
        component: JobFormComponent,
      },
      {
        path: 'jobs/:id',
        component: JobFormComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule { }
