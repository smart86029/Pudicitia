import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { AttendanceRoutingModule } from './attendance-routing.module';
import { AttendanceComponent } from './attendance.component';
import { LeaveFormComponent } from './leave-form/leave-form.component';
import { LeaveListComponent } from './leave-list/leave-list.component';

@NgModule({
  declarations: [
    LeaveFormComponent,
    LeaveListComponent,
    AttendanceComponent,
  ],
  imports: [
    SharedModule,
    AttendanceRoutingModule,
  ],
})
export class AttendanceModule {}
