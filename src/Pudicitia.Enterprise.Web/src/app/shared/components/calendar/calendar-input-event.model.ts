import { CalendarMode } from "./calendar-mode.enum";

export interface CalendarInputEvent<TDate> {
  date: TDate;
  mode: CalendarMode;
}
