import { Guid } from '../../shared/models/guid.model';

export interface Role {
  id: Guid;
  name: string;
  isEnabled: boolean;
  permissionIds: Guid[];
}
