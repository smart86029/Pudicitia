import { Component } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';
import { DateRange } from 'shared/models/date-range-model';

import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent {
  dateRange$ = new BehaviorSubject<DateRange<Date>>({
    start: this.dateAdapter.today(),
    end: this.dateAdapter.today(),
  });
  events$: Observable<CalendarEvent[]> = this.buildCalendarEvents();

  constructor(private dateAdapter: DateAdapter<Date>, private scheduleService: ScheduleService) {}

  private buildCalendarEvents(): Observable<CalendarEvent[]> {
    return this.dateRange$.pipe(switchMap(({ start, end }) => this.scheduleService.getEvents(start, end)));
  }
}
