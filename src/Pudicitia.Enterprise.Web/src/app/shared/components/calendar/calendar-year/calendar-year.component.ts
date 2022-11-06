import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { CalendarCell } from '../calendar-cell';
import { CalendarInputEvent } from '../calendar-input-event.model';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK, MONTHS_IN_YEAR } from '../calendar.constant';

const ROWS_PER_MONTH = 6;

@Component({
  selector: 'app-calendar-year',
  templateUrl: './calendar-year.component.html',
  styleUrls: ['./calendar-year.component.scss'],
})
export class CalendarYearComponent<TDate> implements OnChanges {
  @Input() date: TDate = this.dateAdapter.today();
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent<TDate>>();

  monthNames: string[] = this.dateAdapter.getMonthNames('long');
  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  months: CalendarCell<TDate>[][][] = [];

  constructor(private dateAdapter: DateAdapter<TDate>) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const year = this.dateAdapter.getYear(changes['date'].currentValue as TDate);
      const months: CalendarCell<TDate>[][][] = [];
      for (let month = 0; month < MONTHS_IN_YEAR; month++) {
        const firstDate = this.dateAdapter.createDate(year, month, 1);
        months.push(this.buildMonth(firstDate));
      }
      this.months = months;
    }
  }

  selectMonth(month: number): void {
    const year = this.dateAdapter.getYear(this.date);
    const date = this.dateAdapter.createDate(year, month, 1);
    this.inputChange.emit({ date, mode: CalendarMode.Month });
  }

  selectDate(date: TDate): void {
    this.inputChange.emit({ date, mode: CalendarMode.Day });
  }

  private buildDayOfWeekNames(): string[] {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('narrow');
    return dayOfWeekNames.slice(firstDayOfWeek).concat(dayOfWeekNames.slice(0, firstDayOfWeek));
  }

  private buildMonth(firstDate: TDate): CalendarCell<TDate>[][] {
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
        isToday: false,
      });
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
    const remain = DAYS_IN_WEEK - row.length + (ROWS_PER_MONTH - rows.length - 1) * DAYS_IN_WEEK;
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
        isToday: false,
      });
    }

    rows.push(row);
    return rows;
  }
}
