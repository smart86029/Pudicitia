import { Guid } from 'shared/models/guid.model';

import { Gender } from './gender.enum';
import { MaritalStatus } from './marital-status.enum';

export interface Employee {
  id: Guid;
  name: string;
  displayName: string;
  birthDate: Date;
  gender: Gender;
  maritalStatus: MaritalStatus;
  departmentId: Guid;
  departmentName: string;
  jobId: Guid;
  jobTitle: string;
  startOn: Date;
}
