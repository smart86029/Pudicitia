import { CalendarMode } from './calendar-mode.enum';

export interface CalendarInputEvent {
  date: Date;
  mode: CalendarMode;
}
