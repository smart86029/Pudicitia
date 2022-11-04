import { CalendarEvent } from "./calendar-event.model";

export interface CalendarCell<TDate> {
  day: number;
  date: TDate;
  isEnabled: boolean;
  isToday: boolean;
  events?: CalendarEvent[];
}

export class DefaultCalendarCell<TDate> implements CalendarCell<TDate> {
  day = 0;
  date: TDate = {} as TDate;
  isEnabled = false;
  isToday = false;
  events?: CalendarEvent[] = [];
}
