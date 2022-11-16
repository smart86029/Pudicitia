import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, combineLatest, debounceTime, Observable, switchMap, tap } from 'rxjs';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../../../core/attendance/leave-type.enum';
import { Leave } from '../leave.model';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent {
  LeaveType = LeaveType;
  ApprovalStatus = ApprovalStatus;

  isLoading = true;
  displayedColumns = [
    'sn',
    'employee-name',
    'type',
    'started-on',
    'ended-on',
    'approval-status',
    'action',
  ];
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  interval$ = new BehaviorSubject<Interval | undefined>(undefined);
  approvalStatus$ = new BehaviorSubject<ApprovalStatus | undefined>(undefined);
  leaves$: Observable<PaginationOutput<Leave>> = this.buildLeaves();

  constructor(private attendanceService: AttendanceService) {}

  private buildLeaves(): Observable<PaginationOutput<Leave>> {
    return combineLatest([
      this.page$,
      this.interval$,
      this.approvalStatus$,
    ]).pipe(
      debounceTime(10),
      tap(() => (this.isLoading = true)),
      switchMap(
        ([
          page,
          interval,
          approvalStatus,
        ]) => this.attendanceService.getLeaves(page, interval, approvalStatus),
      ),
      tap(() => (this.isLoading = false)),
    );
  }
}
