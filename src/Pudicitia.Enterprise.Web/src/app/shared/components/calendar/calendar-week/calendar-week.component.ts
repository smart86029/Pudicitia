import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { addDays, endOfWeek, format, getDate, isFuture, isToday, startOfWeek } from 'date-fns';

import { CalendarCell } from '../calendar-cell';
import { CalendarEvent } from '../calendar-event.model';
import { CalendarInputEvent } from '../calendar-input-event.model';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK, HOURS_IN_DAY } from '../calendar.constant';

@Component({
  selector: 'app-calendar-week',
  templateUrl: './calendar-week.component.html',
  styleUrls: ['./calendar-week.component.scss'],
})
export class CalendarWeekComponent implements OnChanges {
  @Input() date = new Date();
  @Input() events: CalendarEvent[] = [];
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent>();
  @Output() readonly intervalChange = new EventEmitter<Interval>();

  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  hours: number[] = this.buildHours();
  row: CalendarCell[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const interval = this.getInterval(changes['date'].currentValue as Date);
      this.row = this.buildRow(interval);
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

  private buildHours(): number[] {
    const hours: number[] = [];
    for (let i = 1; i < HOURS_IN_DAY; i++) {
      hours.push(i);
    }
    return hours;
  }

  private getInterval(date: Date): Interval {
    const start = startOfWeek(date);
    const end = endOfWeek(date);
    return { start, end };
  }

  private buildRow({ start }: Interval): CalendarCell[] {
    const row: CalendarCell[] = [];
    for (let i = 0; i < DAYS_IN_WEEK; i++) {
      const date = addDays(start, i);
      row.push({
        day: getDate(date),
        date,
        isEnabled: isToday(date) || isFuture(date),
        isToday: isToday(date),
      });
    }
    return row;
  }

  private setEvents(events: CalendarEvent[]): void {
    const dictionary = new Map<number, CalendarEvent[]>();
    events.forEach(event => {
      const key = new Date(event.startedOn).getDate();
      if (!dictionary.has(key)) {
        dictionary.set(key, []);
      }
      dictionary.get(key)!.push(event);
    });

    this.row.forEach(cell => {
      const key = cell.day;
      if (dictionary.has(key)) {
        cell.events = dictionary.get(key)!;
      }
    });
  }
}
