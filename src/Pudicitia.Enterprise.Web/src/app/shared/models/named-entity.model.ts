import { Guid } from './guid.model';

export interface NamedEntity {
  id: Guid;
  name: string;
}
