import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DepartmentListComponent } from './department-list/department-list.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';
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
        path: 'employees',
        component: EmployeeListComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationRoutingModule { }
