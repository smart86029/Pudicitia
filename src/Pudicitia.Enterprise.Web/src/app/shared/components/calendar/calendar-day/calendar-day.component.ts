import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';

import { CalendarCell, DefaultCalendarCell } from '../calendar-cell';
import { CalendarEvent } from '../calendar-event.model';
import { HOURS_IN_DAY } from '../calendar.constant';

@Component({
  selector: 'app-calendar-day',
  templateUrl: './calendar-day.component.html',
  styleUrls: ['./calendar-day.component.scss'],
})
export class CalendarDayComponent<TDate> implements OnChanges {
  @Input() date: TDate = this.dateAdapter.today();
  @Input() events: CalendarEvent[] = [];

  hours: number[] = this.buildHours();
  dayOfWeekName = '';
  calendarDate: CalendarCell<TDate> = new DefaultCalendarCell<TDate>();

  constructor(private dateAdapter: DateAdapter<TDate>) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const date = changes['date'].currentValue as TDate;
      this.dayOfWeekName = this.buildDayOfWeekName(date);
      this.calendarDate = this.buildCell(date);
    }
    if (changes['events']) {
      this.setEvents(changes['events'].currentValue);
    }
  }

  private buildHours(): number[] {
    const hours: number[] = [];
    for (let i = 1; i < HOURS_IN_DAY; i++) {
      hours.push(i);
    }
    return hours;
  }

  private buildDayOfWeekName(date: TDate): string {
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('long');
    return dayOfWeekNames[this.dateAdapter.getDayOfWeek(date)];
  }

  private buildCell(date: TDate): CalendarCell<TDate> {
    const today = this.dateAdapter.today();
    return {
      day: this.dateAdapter.getDate(date),
      date,
      isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
      isToday: this.dateAdapter.sameDate(date, today),
    };
  }

  private setEvents(events: CalendarEvent[]): void {
    if (this.calendarDate) {
      this.calendarDate.events = events;
    }
  }
}
