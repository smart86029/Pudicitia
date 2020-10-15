import { Guid } from 'src/app/core/guid';

export class Permission {
  id: Guid;
  code: string;
  name: string;
  description: string;
  isEnabled: boolean;
}
