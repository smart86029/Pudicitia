import { Component, EventEmitter, Input, Output } from '@angular/core';
import { addDays, addMonths, addWeeks, addYears, format } from 'date-fns';
import { BehaviorSubject, map, Observable } from 'rxjs';

import { CalendarEvent } from './calendar-event.model';
import { CalendarInputEvent } from './calendar-input-event.model';
import { CalendarMode } from './calendar-mode.enum';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
})
export class CalendarComponent {
  @Input() events: CalendarEvent[] = [];
  @Output() readonly intervalChange = new EventEmitter<Interval>();

  CalendarMode = CalendarMode;

  input$ = new BehaviorSubject<CalendarInputEvent>({ date: new Date(), mode: CalendarMode.Month });
  title$: Observable<string> = this.buildTitle();

  onModeChange = (mode: CalendarMode): void => {
    const date = this.input$.value.date;
    this.input$.next({ date, mode });
  };

  today(): void {
    const date = new Date();
    const mode = this.input$.value.mode;
    this.input$.next({ date, mode });
  }

  adjustDate(isPositive: boolean): void {
    const value = this.input$.value;
    const sign = isPositive ? 1 : -1;
    let date = value.date;
    switch (value.mode) {
      case CalendarMode.Day:
        date = addDays(date, 1 * sign);
        break;
      case CalendarMode.Week:
        date = addWeeks(date, 1 * sign);
        break;
      case CalendarMode.Month:
      default:
        date = addMonths(date, 1 * sign);
        break;
      case CalendarMode.Year:
        date = addYears(date, 1 * sign);
        break;
    }
    this.input$.next({ date, mode: value.mode });
  }

  private buildTitle(): Observable<string> {
    return this.input$.pipe(
      map(({ date, mode }) => {
        switch (mode) {
          case CalendarMode.Day:
            return format(date, 'MMMM dd, yyyy');
          case CalendarMode.Week:
            return `Week ${format(date, 'ww, MMMM yyyy')}`;
          case CalendarMode.Month:
          default:
            return format(date, 'MMMM yyyy');
          case CalendarMode.Year:
            return format(date, 'yyyy');
        }
      }),
    );
  }
}
