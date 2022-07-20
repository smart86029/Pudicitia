import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ApprovalStatus } from '../approval-status.enum';

import { AttendanceService } from '../attendance.service';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent {
  displayedColumns = ['sn', 'type', 'startedOn', 'endedOn', 'approvalStatus', 'action'];
  approvalStatus = ApprovalStatus;
  startedOn?: Date;
  endedOn?: Date;
  approvalStatusValue?: ApprovalStatus;

  constructor(
    private attendanceService: AttendanceService,
  ) { }

  getLeaves = (pageEvent: PageEvent) => this.attendanceService.getLeaves(
    pageEvent,
    this.startedOn,
    this.endedOn,
    this.approvalStatusValue,
  );
}
