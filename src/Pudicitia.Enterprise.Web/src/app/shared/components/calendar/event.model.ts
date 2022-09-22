import { Guid } from "shared/models/guid.model"

export interface Event {
  id: Guid;
  title: string;
  startedOn: Date;
  endedOn: Date;
  isAllDay: boolean;
}
