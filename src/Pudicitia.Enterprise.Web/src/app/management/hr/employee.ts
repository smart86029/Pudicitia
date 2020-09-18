import { Guid } from '../../core/guid';

export class Employee {
  id: Guid;
  name: string;
  displayName: string;
  birthDate: Date;
  //   gender: Gender = Gender.notKnown;
  //   maritalStatus: MaritalStatus = MaritalStatus.notKnown;
  departmentId: Guid;
  jobTitleId: Guid;
  startOn: Date;
}
