import { Guid } from 'shared/models/guid.model';

import { ApprovalStatus } from './approval-status.enum';
import { LeaveType } from './leave-type.enum';

export interface Leave {
  id: Guid;
  type: LeaveType;
  startedOn: Date;
  endedOn: Date;
  reason: string;
  approvalStatus: ApprovalStatus;
  createdOn: Date;
  employeeId: Guid;
  employeeName: string;
}
