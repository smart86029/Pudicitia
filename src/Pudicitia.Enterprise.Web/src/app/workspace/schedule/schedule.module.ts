import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { ScheduleRoutingModule } from './schedule-routing.module';
import { ScheduleComponent } from './schedule.component';
import { EventDialogComponent } from './event-dialog/event-dialog.component';

@NgModule({
  declarations: [
    ScheduleComponent,
    EventDialogComponent,
  ],
  imports: [
    SharedModule,
    ScheduleRoutingModule,
  ],
})
export class ScheduleModule {}
