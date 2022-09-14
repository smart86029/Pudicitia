import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { ScheduleRoutingModule } from './schedule-routing.module';
import { ScheduleComponent } from './schedule.component';

@NgModule({
  declarations: [
    ScheduleComponent,
  ],
  imports: [
    SharedModule,
    ScheduleRoutingModule,
  ],
})
export class ScheduleModule { }
