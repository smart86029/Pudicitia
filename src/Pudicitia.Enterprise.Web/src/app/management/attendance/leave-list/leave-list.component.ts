import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, combineLatest, switchMap } from 'rxjs';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../leave-type.enum';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent {
  displayedColumns = ['sn', 'employee-name', 'type', 'started-on', 'ended-on', 'approval-status', 'action'];
  leaveType = LeaveType;
  approvalStatus = ApprovalStatus;
  startedOn$ = new BehaviorSubject<Date | undefined>(undefined);
  endedOn$ = new BehaviorSubject<Date | undefined>(undefined);
  approvalStatus$ = new BehaviorSubject<ApprovalStatus | undefined>(undefined);

  constructor(
    private attendanceService: AttendanceService,
  ) { }

  getLeaves = (pageEvent: PageEvent) => combineLatest([
    this.startedOn$,
    this.endedOn$,
    this.approvalStatus$,
  ]).pipe(
    switchMap(([startedOn, endedOn, approvalStatus]) => this.attendanceService.getLeaves(
      pageEvent,
      startedOn,
      endedOn,
      approvalStatus,
    )),
  );
}
