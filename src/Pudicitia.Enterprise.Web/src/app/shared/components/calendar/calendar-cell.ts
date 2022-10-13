import { CalendarEvent } from "./calendar-event.model";

export interface CalendarCell<TDate> {
  day: number;
  date: TDate;
  isEnabled: boolean;
  isToday: boolean;
  events?: CalendarEvent[];
}

export class DefaultCalendarCell<TDate> implements CalendarCell<TDate> {
  day: number;
  date: TDate;
  isEnabled: boolean;
  isToday: boolean;
  events?: CalendarEvent[];

  constructor(
    value?: number,
    isEnabled?: boolean,
    isToday?: boolean,
    date?: TDate,
    events?: CalendarEvent[],
  ) {
    this.day = value || 0;
    this.isEnabled = isEnabled || false;
    this.isToday = isToday || false;
    this.date = date || <TDate>{};
    this.events = events || [];
  }
}
