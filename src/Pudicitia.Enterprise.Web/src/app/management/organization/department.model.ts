import { Guid } from "shared/models/guid.model";

export interface Department {
  id: Guid;
  name: string;
  isEnabled: boolean;
  parentId?: Guid;
  children?: Department[];
}
