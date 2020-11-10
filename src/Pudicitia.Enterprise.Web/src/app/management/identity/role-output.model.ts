import { NamedEntity } from '../../shared/models/named-entity.model';
import { Role } from './role.model';

export interface RoleOutput {
  role: Role;
  permissions: NamedEntity[];
}
