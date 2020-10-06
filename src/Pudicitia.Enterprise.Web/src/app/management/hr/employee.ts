import { Guid } from '../../core/guid';
import { Gender } from './gender';
import { MaritalStatus } from './marital-status';

export class Employee {
  id: Guid;
  name: string;
  displayName: string;
  birthDate: Date;
  gender: Gender = Gender.notKnown;
  maritalStatus: MaritalStatus = MaritalStatus.notKnown;
  departmentId: Guid;
  jobId: Guid;
  startOn: Date;
}
