import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { formatISO } from 'date-fns';
import { Observable } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { ApprovalStatus } from './approval-status.enum';
import { Leave } from './leave.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private urlLeaves = 'api/attendance/leaves';

  constructor(private httpClient: HttpClient) {}

  getLeaves(
    page: { pageIndex: number; pageSize: number },
    interval?: Interval,
    approvalStatus?: ApprovalStatus,
  ): Observable<PaginationOutput<Leave>> {
    const params = new HttpParams({
      fromObject: {
        ...page,
        startedOn: interval ? formatISO(interval.start) : '',
        endedOn: interval ? formatISO(interval.end) : '',
        approvalStatus: approvalStatus || '',
      },
    });
    return this.httpClient.get<PaginationOutput<Leave>>(this.urlLeaves, { params });
  }

  getLeave(id: Guid): Observable<Leave> {
    return this.httpClient.get<Leave>(`${this.urlLeaves}/${id}`);
  }
}
