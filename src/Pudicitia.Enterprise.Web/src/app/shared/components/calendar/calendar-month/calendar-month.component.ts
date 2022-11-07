import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import {
  addDays,
  endOfMonth,
  endOfWeek,
  format,
  getDate,
  getMonth,
  isBefore,
  isSameMonth,
  isToday,
  startOfMonth,
  startOfWeek,
  toDate,
} from 'date-fns';

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
export class CalendarMonthComponent implements OnChanges {
  @Input() date = new Date();
  @Input() events: CalendarEvent[] = [];
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent>();
  @Output() readonly intervalChange = new EventEmitter<Interval>();

  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  rows: CalendarCell[][] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const interval = this.getInterval(changes['date'].currentValue as Date);
      this.rows = this.buildRows(interval);
      this.intervalChange.emit(interval);
    }
    if (changes['events']) {
      this.setEvents(changes['events'].currentValue);
    }
  }

  selectDate(date: Date): void {
    this.inputChange.emit({ date, mode: CalendarMode.Day });
  }

  private buildDayOfWeekNames(): string[] {
    const dayOfWeekNames: string[] = [];
    const start = startOfWeek(this.date);
    for (let i = 0; i < DAYS_IN_WEEK; i++) {
      dayOfWeekNames.push(format(addDays(start, i), 'EEEE'));
    }
    return dayOfWeekNames;
  }

  private getInterval(date: Date): Interval {
    const firstDate = startOfMonth(date);
    const start = startOfWeek(firstDate);
    const lastDate = endOfMonth(date);
    const end = endOfWeek(lastDate);
    return { start, end };
  }

  private buildRows({ start, end }: Interval): CalendarCell[][] {
    const rows = [];
    let row: CalendarCell[] = [];
    let date = toDate(start);
    while (isBefore(date, end)) {
      row.push({
        day: getDate(date),
        date,
        isEnabled: isSameMonth(date, this.date),
        isToday: isToday(date),
      });
      date = addDays(date, 1);
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
      const key = getMonth(date) * 100 + getDate(date);
      if (!dictionary.has(key)) {
        dictionary.set(key, []);
      }
      dictionary.get(key)!.push(event);
    });
    this.rows.forEach(row => {
      row.forEach(cell => {
        const key = getMonth(cell.date) * 100 + getDate(cell.date);
        if (dictionary.has(key)) {
          cell.events = dictionary.get(key)!;
        }
      });
    });
  }
}
