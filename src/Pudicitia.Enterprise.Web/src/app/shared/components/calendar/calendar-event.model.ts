import { Guid } from 'shared/models/guid.model';

export interface CalendarEvent {
  id: Guid;
  title: string;
  startedOn: Date;
  endedOn: Date;
  isAllDay: boolean;
}
