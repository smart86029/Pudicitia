import { Component } from '@angular/core';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';

import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent {
  interval$ = new BehaviorSubject<Interval>({ start: new Date(), end: new Date() });
  events$: Observable<CalendarEvent[]> = this.buildCalendarEvents();

  constructor(private scheduleService: ScheduleService) {}

  private buildCalendarEvents(): Observable<CalendarEvent[]> {
    return this.interval$.pipe(switchMap(interval => this.scheduleService.getEvents(interval)));
  }
}
