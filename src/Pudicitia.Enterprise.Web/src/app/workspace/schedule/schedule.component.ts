import { Component } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import * as moment from 'moment';

import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent<TDate> {
  constructor(
    private dateAdapter: DateAdapter<TDate>,
    private scheduleService: ScheduleService,
  ) { }

  getItems = (startedOn: TDate, endedOn: TDate) => {
    const startDate = moment.isDate(startedOn) ? startedOn : new Date(this.dateAdapter.toIso8601(startedOn));
    const endDate = moment.isDate(endedOn) ? endedOn : new Date(this.dateAdapter.toIso8601(endedOn));
    return this.scheduleService.getEvents(startDate, endDate);
  };
}
