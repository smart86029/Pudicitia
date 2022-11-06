import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { DateRange } from 'shared/models/date-range-model';

import { CalendarEvent } from './calendar-event.model';
import { CalendarInputEvent } from './calendar-input-event.model';
import { CalendarMode } from './calendar-mode.enum';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
})
export class CalendarComponent<TDate> {
  @Input() events: CalendarEvent[] = [];
  @Output() readonly dateRangeChange = new EventEmitter<DateRange<TDate>>();

  CalendarMode = CalendarMode;

  input$ = new BehaviorSubject<CalendarInputEvent<TDate>>({ date: this.dateAdapter.today(), mode: CalendarMode.Month });
  title$: Observable<string> = this.buildTitle();

  constructor(private dateAdapter: DateAdapter<TDate>) {}

  onModeChange = (mode: CalendarMode): void => {
    const date = this.input$.value.date;
    this.input$.next({ date, mode });
  };

  today(): void {
    const date = this.dateAdapter.today();
    const mode = this.input$.value.mode;
    this.input$.next({ date, mode });
  }

  adjustDate(isPositive: boolean): void {
    const value = this.input$.value;
    const sign = isPositive ? 1 : -1;
    let date = value.date;
    switch (value.mode) {
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
    this.input$.next({ date, mode: value.mode });
  }

  private buildTitle(): Observable<string> {
    return this.input$.pipe(
      map(({ date, mode }) => {
        switch (mode) {
          case CalendarMode.Day:
            return this.dateAdapter.format(date, 'MMMM dd, yyyy');
          case CalendarMode.Week:
            return `Week ${this.dateAdapter.format(date, 'ww, MMMM yyyy')}`;
          case CalendarMode.Month:
          default:
            return this.dateAdapter.format(date, 'MMMM yyyy');
          case CalendarMode.Year:
            return this.dateAdapter.format(date, 'yyyy');
        }
      }),
    );
  }
}
