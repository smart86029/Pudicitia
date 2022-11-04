import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { DateRange } from 'shared/models/date-range-model';

import { CalendarCell } from '../calendar-cell';
import { CalendarEvent } from '../calendar-event.model';
import { CalendarInputEvent } from '../calendar-input-event.model';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK } from '../calendar.constant';

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
})
export class CalendarMonthComponent<TDate> implements OnChanges {
  @Input() date: TDate = this.dateAdapter.today();
  @Input() events: CalendarEvent[] = [];
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent<TDate>>();
  @Output() readonly dateRangeChange = new EventEmitter<DateRange<TDate>>();

  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  rows: CalendarCell<TDate>[][] = [];

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const dateRange = this.getDateRange(changes['date'].currentValue as TDate);
      this.rows = this.buildRows(dateRange);
      this.dateRangeChange.emit(dateRange);
    }

    if (changes['events']) {
      this.setEvents(changes['events'].currentValue);
    }
  }

  selectDate(date: TDate): void {
    this.inputChange.emit({ date, mode: CalendarMode.Day });
  }

  private buildDayOfWeekNames(): string[] {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('long');
    return dayOfWeekNames.slice(firstDayOfWeek).concat(dayOfWeekNames.slice(0, firstDayOfWeek));
  }

  private getDateRange(date: TDate): DateRange<TDate> {
    const year = this.dateAdapter.getYear(date);
    const month = this.dateAdapter.getMonth(date);
    const firstDate = this.dateAdapter.createDate(year, month, 1);
    const dayOfWeek = this.dateAdapter.getDayOfWeek(firstDate);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_IN_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_IN_WEEK;
    const start = this.dateAdapter.addCalendarDays(firstDate, -firstWeekOffset);

    const daysInMonth = this.dateAdapter.getNumDaysInMonth(firstDate);
    const lastDate = this.dateAdapter.createDate(year, month, daysInMonth);
    const lastDayOfWeek = this.dateAdapter.getDayOfWeek(lastDate);
    const remain = (DAYS_IN_WEEK - lastDayOfWeek + firstDayOfWeek) % DAYS_IN_WEEK;
    const end = this.dateAdapter.addCalendarDays(lastDate, remain);

    return { start, end };
  }

  private buildRows({ start, end }: DateRange<TDate>): CalendarCell<TDate>[][] {
    const rows = [];
    const month = this.dateAdapter.getMonth(this.date);
    const today = this.dateAdapter.today();
    let row: CalendarCell<TDate>[] = [];
    let date = start;
    while (this.dateAdapter.compareDate(date, end) < 0) {
      row.push({
        day: this.dateAdapter.getDate(date),
        date,
        isEnabled: this.dateAdapter.getMonth(date) === month,
        isToday: this.dateAdapter.sameDate(date, today),
      });
      date = this.dateAdapter.addCalendarDays(date, 1);
      if (row.length == DAYS_IN_WEEK) {
        rows.push(row);
        row = [];
      }
    }

    return rows;
  }

  private setEvents(events: CalendarEvent[]): void {
    const dictionary = new Map<number, CalendarEvent[]>();
    events.forEach(event => {
      const date = new Date(event.startedOn);
      const key = date.getMonth() * 100 + date.getDate();
      if (!dictionary.has(key)) {
        dictionary.set(key, []);
      }
      dictionary.get(key)!.push(event);
    });
    this.rows.forEach(row => {
      row.forEach(cell => {
        const key = this.dateAdapter.getMonth(cell.date) * 100 + this.dateAdapter.getDate(cell.date);
        if (dictionary.has(key)) {
          cell.events = dictionary.get(key)!;
        }
      })
    })
  }
}
