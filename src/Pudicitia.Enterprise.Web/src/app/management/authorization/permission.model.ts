import { Guid } from 'src/app/shared/models/guid.model';

export interface Permission {
  id: Guid;
  code: string;
  name: string;
  description: string;
  isEnabled: boolean;
}
