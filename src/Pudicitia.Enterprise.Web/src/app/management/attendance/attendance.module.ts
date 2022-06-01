import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { AttendanceRoutingModule } from './attendance-routing.module';
import { LeaveListComponent } from './leave-list/leave-list.component';

@NgModule({
  declarations: [
    LeaveListComponent,
  ],
  imports: [
    SharedModule,
    AttendanceRoutingModule,
  ],
})
export class AttendanceModule { }
