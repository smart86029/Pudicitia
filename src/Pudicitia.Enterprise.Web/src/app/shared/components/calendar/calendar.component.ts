import { Component, Input, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, combineLatest, Observable, tap } from 'rxjs';

import { CalendarMode } from './calendar-mode.enum';
import { Event } from './event.model';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
})
export class CalendarComponent<TDate> implements OnInit {
  calendarMode = CalendarMode;
  calendarMode$ = new BehaviorSubject<CalendarMode>(CalendarMode.Month);
  date$ = new BehaviorSubject(this.dateAdapter.today());
  title = '';

  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<Event[]>;

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    combineLatest([
      this.calendarMode$,
      this.date$,
    ])
      .pipe(
        tap(([calendarMode, date]) => this.title = this.getTitle(calendarMode, date)),
      )
      .subscribe();
  }

  adjustDate(isPositive: boolean): void {
    let date = this.date$.value;
    const sign = isPositive ? 1 : -1;
    switch (this.calendarMode$.value) {
      case CalendarMode.Day:
        date = this.dateAdapter.addCalendarDays(date, 1 * sign);
        break;
      case CalendarMode.Week:
        date = this.dateAdapter.addCalendarDays(date, 7 * sign);
        break;
      case CalendarMode.Month:
      default:
        date = this.dateAdapter.addCalendarMonths(date, 1 * sign);
        break;
      case CalendarMode.Year:
        date = this.dateAdapter.addCalendarYears(date, 1 * sign);
        break;
    }
    this.date$.next(date);
  }

  private getTitle(calendarMode: CalendarMode, date: TDate): string {
    switch (calendarMode) {
      case CalendarMode.Day:
        return this.dateAdapter.format(date, 'MMMM DD, YYYY');
      case CalendarMode.Week:
        return `Week ${this.dateAdapter.format(date, 'WW, MMMM YYYY')}`;
      case CalendarMode.Month:
      default:
        return this.dateAdapter.format(date, 'MMMM YYYY');
      case CalendarMode.Year:
        return this.dateAdapter.format(date, 'YYYY');
    }
  }
}
