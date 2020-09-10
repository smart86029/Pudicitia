import { Guid } from '../../core/guid';

export class Department {
  id: Guid;
  name: string;
  parentId?: Guid;
  children?: Department[] = [];
}
