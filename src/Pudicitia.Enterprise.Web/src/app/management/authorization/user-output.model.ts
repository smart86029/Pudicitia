import { NamedEntity } from 'shared/models/named-entity.model';

import { User } from './user.model';

export interface UserOutput {
  user: User;
  roles: NamedEntity[];
}
