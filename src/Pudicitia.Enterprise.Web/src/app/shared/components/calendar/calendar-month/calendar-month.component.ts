import { Component, Input } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { CalendarCell } from '../calendar-cell';

const DAYS_PER_WEEK = 7;

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
})
export class CalendarMonthComponent<TDate>  {
  weekdays: string[] = [];
  rows: CalendarCell<TDate>[][] = [];
  year: number;
  month: number;
  firstWeekOffset: number;

  @Input() date: TDate;

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) {
    this.date = this.dateAdapter.today();
    this.year = this.dateAdapter.getYear(this.date);
    this.month = this.dateAdapter.getMonth(this.date);

    const firstDayOfMonth = this.dateAdapter.createDate(this.year, this.month, 1);
    const dayOfWeek = this.dateAdapter.getDayOfWeek(firstDayOfMonth);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    this.firstWeekOffset = (DAYS_PER_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_PER_WEEK;

    this.initWeekdays();
    this.createCells();
  }

  private initWeekdays(): void {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const weekdays = this.dateAdapter.getDayOfWeekNames('long');
    this.weekdays = weekdays.slice(firstDayOfWeek).concat(weekdays.slice(0, firstDayOfWeek));
  }

  private createCells(): void {
    let row = [];
    const dateNames = this.dateAdapter.getDateNames();
    const firstDayOfMonth = this.dateAdapter.createDate(this.year, this.month, 1);
    for (let i = this.firstWeekOffset; i > 0; i--) {
      const date = this.dateAdapter.addCalendarDays(firstDayOfMonth, -i);
      const value = this.dateAdapter.getDate(date);
      row.push({
        value: value,
        displayValue: dateNames[value - 1],
        isEnabled: false,
        date: date,
      })
    }

    const daysInMonth = this.dateAdapter.getNumDaysInMonth(this.date);
    for (let i = 1, cell = this.firstWeekOffset; i <= daysInMonth; i++, cell++) {
      if (cell == DAYS_PER_WEEK) {
        this.rows.push(row);
        row = [];
        cell = 0;
      }
      const date = this.dateAdapter.createDate(this.year, this.month, i);
      row.push({
        value: i,
        displayValue: dateNames[i - 1],
        isEnabled: true,
        date: date,
      });
    }

    const lastDayofMonth = this.dateAdapter.createDate(this.year, this.month, daysInMonth);
    for (let i = 1; i <= DAYS_PER_WEEK - row.length; i++) {
      const date = this.dateAdapter.addCalendarDays(lastDayofMonth, i);
      row.push({
        value: i,
        displayValue: dateNames[i - 1],
        isEnabled: false,
        date: date,
      });
    }

    this.rows.push(row);
  }
}
