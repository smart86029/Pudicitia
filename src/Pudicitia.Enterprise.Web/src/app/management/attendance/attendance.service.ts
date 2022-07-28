import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { ApprovalStatus } from './approval-status.enum';
import { Leave } from './leave.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private urlLeaves = 'api/attendance/leaves';

  constructor(private httpClient: HttpClient) { }

  getLeaves(
    page: { pageIndex: number, pageSize: number },
    startedOn?: Date,
    endedOn?: Date,
    approvalStatus?: ApprovalStatus,
  ): Observable<PaginationOutput<Leave>> {
    const params = new HttpParams({
      fromObject: {
        ...page,
        startedOn: startedOn?.toISOString() || '',
        endedOn: endedOn?.toISOString() || '',
        approvalStatus: approvalStatus || '',
      },
    });
    return this.httpClient.get<PaginationOutput<Leave>>(this.urlLeaves, { params });
  }
}
