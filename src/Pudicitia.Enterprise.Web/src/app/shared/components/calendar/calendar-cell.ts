import { getDate } from 'date-fns';

import { CalendarEvent } from './calendar-event.model';

export interface CalendarCell {
  day: number;
  date: Date;
  isEnabled: boolean;
  isToday: boolean;
  events?: CalendarEvent[];
}

export class DefaultCalendarCell implements CalendarCell {
  day = getDate(new Date());
  date = new Date();
  isEnabled = false;
  isToday = true;
  events?: CalendarEvent[] = [];
}
