import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { addDays, addMonths, format, getDate, getYear, isToday, startOfWeek, startOfYear } from 'date-fns';

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
export class CalendarYearComponent implements OnChanges {
  @Input() date = new Date();
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent>();

  monthNames: string[] = this.buildMonthNames();
  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  months: CalendarCell[][][] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const year = getYear(changes['date'].currentValue as Date);
      const months: CalendarCell[][][] = [];
      for (let month = 0; month < MONTHS_IN_YEAR; month++) {
        months.push(this.buildMonth(new Date(year, month, 1)));
      }
      this.months = months;
    }
  }

  selectMonth(month: number): void {
    const year = getYear(this.date);
    const date = new Date(year, month, 1);
    this.inputChange.emit({ date, mode: CalendarMode.Month });
  }

  selectDate(date: Date): void {
    this.inputChange.emit({ date, mode: CalendarMode.Day });
  }

  private buildMonthNames(): string[] {
    const monthNames: string[] = [];
    const start = startOfYear(this.date);
    for (let i = 0; i < MONTHS_IN_YEAR; i++) {
      monthNames.push(format(addMonths(start, i), 'MMMM'));
    }
    return monthNames;
  }

  private buildDayOfWeekNames(): string[] {
    const dayOfWeekNames: string[] = [];
    const start = startOfWeek(this.date);
    for (let i = 0; i < DAYS_IN_WEEK; i++) {
      dayOfWeekNames.push(format(addDays(start, i), 'EEEE'));
    }
    return dayOfWeekNames;
  }

  private buildMonth(date: Date): CalendarCell[][] {
    const rows: CalendarCell[][] = [];
    const count = DAYS_IN_WEEK * ROWS_PER_MONTH;
    const start = startOfWeek(date);
    let row: CalendarCell[] = [];
    for (let i = 0; i <= count; i++) {
      const date = addDays(start, i);
      row.push({
        day: getDate(date),
        date,
        isEnabled: true,
        isToday: isToday(date),
      });
      if (row.length == DAYS_IN_WEEK) {
        rows.push(row);
        row = [];
      }
    }
    return rows;
  }
}
