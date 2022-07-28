import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, switchMap } from 'rxjs';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../leave-type.enum';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent {
  displayedColumns = ['sn', 'employeeName', 'type', 'startedOn', 'endedOn', 'approvalStatus', 'action'];
  leaveType = LeaveType;
  approvalStatus = ApprovalStatus;
  startedOn?: Date;
  endedOn?: Date;
  approvalStatusValue?: ApprovalStatus;
  value$ = new BehaviorSubject<[Date | undefined, Date | undefined, ApprovalStatus | undefined]>([undefined, undefined, undefined]);

  constructor(
    private attendanceService: AttendanceService,
  ) { }

  getLeaves = (pageEvent: PageEvent) => this.value$.pipe(
    switchMap(([startedOn, endedOn, approvalStatus]) => this.attendanceService.getLeaves(
      pageEvent,
      startedOn,
      endedOn,
      approvalStatus,
    )),
  );

  search(): void {
    this.value$.next([this.startedOn, this.endedOn, this.approvalStatusValue]);
  }
}
