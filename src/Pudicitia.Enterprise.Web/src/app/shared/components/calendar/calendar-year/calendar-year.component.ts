import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { CalendarCell } from '../calendar-cell';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK, MONTHS_IN_YEAR } from '../calendar.constant';
import { CalendarEvent } from '../calendar-event.model';

const ROW_PER_MONTH = 6;

@Component({
  selector: 'app-calendar-year',
  templateUrl: './calendar-year.component.html',
  styleUrls: ['./calendar-year.component.scss'],
})
export class CalendarYearComponent<TDate> implements OnInit, OnChanges {
  monthNames: string[] = [];
  dayOfWeekNames: string[] = [];
  months: CalendarCell<TDate>[][][] = [];
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<CalendarEvent[]>;
  @Output() dateChange = new EventEmitter<TDate>();
  @Output() modeChange = new EventEmitter<CalendarMode>();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initNames();
    this.date$
      .pipe(
        tap(date => {
          const year = this.dateAdapter.getYear(date);
          this.months = [];
          for (let month = 0; month < MONTHS_IN_YEAR; month++) {
            const firstDate = this.dateAdapter.createDate(year, month, 1);
            this.createCells(firstDate);
          }
        }),
        // switchMap(date => this.getItems(date, date)),
        // tap(events => this.setEvents(events)),
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.date$.next(<TDate>changes['date'].currentValue);
  }

  selectMonth(month: number) {
    const year = this.dateAdapter.getYear(this.date$.value);
    const date = this.dateAdapter.createDate(year, month, 1);
    this.dateChange.emit(date);
    this.modeChange.emit(CalendarMode.Month);
  }

  selectDate(date: TDate) {
    this.dateChange.emit(date);
    this.modeChange.emit(CalendarMode.Day);
  }

  private initNames(): void {
    this.monthNames = this.dateAdapter.getMonthNames('long');

    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('narrow');
    this.dayOfWeekNames = dayOfWeekNames.slice(firstDayOfWeek).concat(dayOfWeekNames.slice(0, firstDayOfWeek));
  }

  private createCells(firstDate: TDate): void {
    const rows: CalendarCell<TDate>[][] = [];
    const today = this.dateAdapter.today();
    const year = this.dateAdapter.getYear(firstDate);
    const month = this.dateAdapter.getMonth(firstDate);
    let row: CalendarCell<TDate>[] = [];

    const dayOfWeek = this.dateAdapter.getDayOfWeek(firstDate);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_IN_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_IN_WEEK;

    for (let i = firstWeekOffset; i > 0; i--) {
      const date = this.dateAdapter.addCalendarDays(firstDate, -i);
      row.push({
        day: this.dateAdapter.getDate(date),
        date,
        isEnabled: false,
        isToday: this.dateAdapter.sameDate(date, today),
      })
    }

    const daysInMonth = this.dateAdapter.getNumDaysInMonth(firstDate);
    for (let day = 1, cell = firstWeekOffset; day <= daysInMonth; day++, cell++) {
      if (cell == DAYS_IN_WEEK) {
        rows.push(row);
        row = [];
        cell = 0;
      }
      const date = this.dateAdapter.createDate(year, month, day);
      row.push({
        day,
        date,
        isEnabled: true,
        isToday: this.dateAdapter.sameDate(date, today),
      });
    }

    const lastDate = this.dateAdapter.createDate(year, month, daysInMonth);
    const remain = DAYS_IN_WEEK - row.length + (ROW_PER_MONTH - rows.length - 1) * DAYS_IN_WEEK;
    for (let day = 1, cell = row.length; day <= remain; day++, cell++) {
      if (cell == DAYS_IN_WEEK) {
        rows.push(row);
        row = [];
        cell = 0;
      }
      const date = this.dateAdapter.addCalendarDays(lastDate, day);
      row.push({
        day,
        date,
        isEnabled: false,
        isToday: this.dateAdapter.sameDate(date, today),
      });
    }

    rows.push(row);
    this.months.push(rows);
  }
}
