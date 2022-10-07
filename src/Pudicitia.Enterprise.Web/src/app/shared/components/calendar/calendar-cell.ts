import { Event } from "./event.model";

export interface CalendarCell<TDate> {
  value: number;
  displayValue: string;
  isEnabled: boolean;
  isToday: boolean;
  date: TDate;
  events?: Event[];
}

export class DefaultCalendarCell<TDate> implements CalendarCell<TDate> {
  value: number;
  displayValue: string;
  isEnabled: boolean;
  isToday: boolean;
  date: TDate;
  events?: Event[];

  constructor(
    value?: number,
    displayValue?: string,
    isEnabled?: boolean,
    isToday?: boolean,
    date?: TDate,
    events?: Event[],
  ) {
    this.value = value || 0;
    this.displayValue = displayValue || '';
    this.isEnabled = isEnabled || false;
    this.isToday = isToday || false;
    this.date = date || <TDate>{};
    this.events = events || [];
  }
}
