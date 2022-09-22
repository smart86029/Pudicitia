import { Event } from "./event.model";

export interface CalendarCell<TDate> {
  value: number;
  displayValue: string;
  isEnabled: boolean;
  date: TDate,
  events?: Event[],
}
