import { Component } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import * as moment from 'moment';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';
import { DateRange } from 'shared/models/date-range-model';

import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent<TDate> {
  dateRange$ = new BehaviorSubject<DateRange<TDate>>({ start: this.dateAdapter.today(), end: this.dateAdapter.today() });
  events$: Observable<CalendarEvent[]> = this.buildCalendarEvents();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
    private scheduleService: ScheduleService,
  ) { }

  private buildCalendarEvents(): Observable<CalendarEvent[]> {
    return this.dateRange$
      .pipe(
        switchMap(({ start, end }) => {
          const startedOn = moment.isDate(start) ? start : new Date(this.dateAdapter.toIso8601(start!));
          const endedOn = moment.isDate(end) ? end : new Date(this.dateAdapter.toIso8601(end!));
          return this.scheduleService.getEvents(startedOn, endedOn);
        }),
      );
  }
}
