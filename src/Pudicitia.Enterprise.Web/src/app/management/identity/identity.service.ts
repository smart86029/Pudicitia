import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PaginationOutput } from '../../core/pagination-output';
import { Role } from './role';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private urlRoles = 'api/identity/roles';

  constructor(private httpClient: HttpClient) {}

  getRoles(
    pageIndex: number,
    pageSize: number
  ): Observable<PaginationOutput<Role>> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());
    return this.httpClient.get<PaginationOutput<Role>>(this.urlRoles, {
      params,
    });
  }
}
