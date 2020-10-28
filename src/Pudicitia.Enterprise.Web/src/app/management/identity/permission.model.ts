import { Guid } from '../../shared/models/guid.model';

export interface Permission {
  id: Guid;
  code: string;
  name: string;
  description: string;
  isEnabled: boolean;
}
