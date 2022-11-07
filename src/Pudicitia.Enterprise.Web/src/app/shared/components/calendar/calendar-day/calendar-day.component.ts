import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { format, getDate, isFuture, isToday } from 'date-fns';

import { CalendarCell, DefaultCalendarCell } from '../calendar-cell';
import { CalendarEvent } from '../calendar-event.model';
import { HOURS_IN_DAY } from '../calendar.constant';

@Component({
  selector: 'app-calendar-day',
  templateUrl: './calendar-day.component.html',
  styleUrls: ['./calendar-day.component.scss'],
})
export class CalendarDayComponent implements OnChanges {
  @Input() date = new Date();
  @Input() events: CalendarEvent[] = [];

  hours: number[] = this.buildHours();
  dayOfWeekName = '';
  calendarDate: CalendarCell = new DefaultCalendarCell();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date']) {
      const date = changes['date'].currentValue as Date;
      this.dayOfWeekName = format(date, 'EEEE');
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

  private buildCell(date: Date): CalendarCell {
    return {
      day: getDate(date),
      date,
      isEnabled: isToday(date) || isFuture(date),
      isToday: isToday(date),
    };
  }

  private setEvents(events: CalendarEvent[]): void {
    if (this.calendarDate) {
      this.calendarDate.events = events;
    }
  }
}
