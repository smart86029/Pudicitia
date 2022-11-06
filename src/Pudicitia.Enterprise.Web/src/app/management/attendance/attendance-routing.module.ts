import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AttendanceComponent } from './attendance.component';
import { LeaveFormComponent } from './leave-form/leave-form.component';
import { LeaveListComponent } from './leave-list/leave-list.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'leaves',
  },
  {
    path: '',
    component: AttendanceComponent,
    children: [
      {
        path: 'leaves',
        component: LeaveListComponent,
      },
      {
        path: 'leaves/:id',
        component: LeaveFormComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AttendanceRoutingModule {}
