import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttendanceComponent } from './attendance.component';
import { LeaveListComponent } from './leave-list/leave-list.component';

const routes: Routes = [
  {
    path: '',
    component: AttendanceComponent,
  },
  {
    path: 'leaves',
    component: LeaveListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AttendanceRoutingModule { }
