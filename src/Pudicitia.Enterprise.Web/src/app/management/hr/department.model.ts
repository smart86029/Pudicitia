import { Guid } from "shared/models/guid.model";

export interface Department {
  id: Guid;
  name: string;
  parentId?: Guid;
  children?: Department[];
}
