import { Guid } from "shared/models/guid.model";

export interface User {
  id: Guid;
  userName: string;
  password: string;
  name: string;
  displayName: string;
  isEnabled: boolean;
  roleIds: Guid[];
}
