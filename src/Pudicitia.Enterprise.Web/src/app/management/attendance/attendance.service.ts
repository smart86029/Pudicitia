import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Leave } from './leave.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private urlLeaves = 'api/attendance/leaves';

  constructor(private httpClient: HttpClient) { }

  getLeaves(
    pageIndex: number,
    pageSize: number,
  ): Observable<PaginationOutput<Leave>> {
    const params = new HttpParams({
      fromObject: {
        pageIndex,
        pageSize,
      },
    });
    return this.httpClient.get<PaginationOutput<Leave>>(this.urlLeaves, { params });
  }
}
