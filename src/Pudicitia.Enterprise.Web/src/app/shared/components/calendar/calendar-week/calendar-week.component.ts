import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { DateRange } from 'shared/models/date-range-model';

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
export class CalendarWeekComponent<TDate> implements OnChanges {
  @Input() date: TDate = this.dateAdapter.today();
  @Input() events: CalendarEvent[] = [];
  @Output() readonly inputChange = new EventEmitter<CalendarInputEvent<TDate>>();
  @Output() readonly dateRangeChange = new EventEmitter<DateRange<TDate>>();

  dayOfWeekNames: string[] = this.buildDayOfWeekNames();
  hours: number[] = this.buildHours();
  row: CalendarCell<TDate>[] = [];

  constructor(private dateAdapter: DateAdapter<TDate>) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const dateRange = this.getDateRange(changes['date'].currentValue as TDate);
      this.row = this.buildRow(dateRange);
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

  private buildHours(): number[] {
    const hours: number[] = [];
    for (let i = 1; i < HOURS_IN_DAY; i++) {
      hours.push(i);
    }
    return hours;
  }

  private getDateRange(date: TDate): DateRange<TDate> {
    const dayOfWeek = this.dateAdapter.getDayOfWeek(date);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_IN_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_IN_WEEK;
    const start = this.dateAdapter.addCalendarDays(date, -firstWeekOffset);
    const end = this.dateAdapter.addCalendarDays(start, DAYS_IN_WEEK - 1);
    return { start, end };
  }

  private buildRow({ start }: DateRange<TDate>): CalendarCell<TDate>[] {
    console.log(1);
    const row: CalendarCell<TDate>[] = [];
    const today = this.dateAdapter.today();
    for (let i = 0; i < DAYS_IN_WEEK; i++) {
      const date = this.dateAdapter.addCalendarDays(start, i);
      row.push({
        day: this.dateAdapter.getDate(date),
        date,
        isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
        isToday: this.dateAdapter.sameDate(date, today),
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
      const key = this.dateAdapter.getDate(cell.date);
      if (dictionary.has(key)) {
        cell.events = dictionary.get(key)!;
      }
    });
  }
}
