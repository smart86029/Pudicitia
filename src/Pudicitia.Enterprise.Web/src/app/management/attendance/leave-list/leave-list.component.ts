import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject, combineLatest, debounceTime, Observable, switchMap, tap } from 'rxjs';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../leave-type.enum';
import { Leave } from '../leave.model';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent {
  isLoading = true;
  displayedColumns = ['sn', 'employee-name', 'type', 'started-on', 'ended-on', 'approval-status', 'action'];
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  leaveType = LeaveType;
  approvalStatus = ApprovalStatus;
  startedOn$ = new BehaviorSubject<Date | undefined>(undefined);
  endedOn$ = new BehaviorSubject<Date | undefined>(undefined);
  approvalStatus$ = new BehaviorSubject<ApprovalStatus | undefined>(undefined);
  leaves$: Observable<PaginationOutput<Leave>> = this.buildLeaves();

  constructor(
    private attendanceService: AttendanceService,
  ) { }

  onPageChange = (page: PageEvent): void => this.page$.next(page);

  private buildLeaves(): Observable<PaginationOutput<Leave>> {
    return combineLatest([
      this.page$,
      this.startedOn$,
      this.endedOn$,
      this.approvalStatus$,
    ])
      .pipe(
        debounceTime(10),
        tap(() => this.isLoading = true),
        switchMap(([page, startedOn, endedOn, approvalStatus]) => this.attendanceService.getLeaves(
          page,
          startedOn,
          endedOn,
          approvalStatus,
        )),
        tap(() => this.isLoading = false),
      );
  }
}
