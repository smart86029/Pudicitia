import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { CalendarCell } from './calendar-cell';
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
  title = '';

  @Input() date: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<Event[]>;
  @Output() readonly monthCellClick = new EventEmitter<CalendarCell<TDate>>();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) {
    this.date = this.dateAdapter.today();
  }

  ngOnInit(): void {
    this.calendarMode$
      .pipe(
        tap(calendarMode => {
          switch (calendarMode) {
            case CalendarMode.Day:
              this.title = this.dateAdapter.format(this.date, 'MMMM DD, YYYY');
              break;
            case CalendarMode.Week:
              this.title = `Week ${this.dateAdapter.format(this.date, 'WW, MMMM YYYY')}`;
              break;
            case CalendarMode.Month:
              this.title = this.dateAdapter.format(this.date, 'MMMM YYYY');
              break;
            case CalendarMode.Year:
              this.title = this.dateAdapter.format(this.date, 'YYYY');
              break;
          }
        }),
      )
      .subscribe();
  }
}
