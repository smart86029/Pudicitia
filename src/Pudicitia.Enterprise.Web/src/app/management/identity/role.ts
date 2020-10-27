import { Guid } from '../../core/guid';

export class Role {
  id: Guid;
  name: string;
  isEnabled: boolean;
  permissionIds: Guid[];
}
