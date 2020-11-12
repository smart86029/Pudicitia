import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Guid } from '../../shared/models/guid.model';
import { PaginationOutput } from '../../shared/models/pagination-output.model';
import { Permission } from './permission.model';
import { RoleOutput } from './role-output.model';
import { Role } from './role.model';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private urlRoles = 'api/identity/roles';
  private urlPermissions = 'api/identity/permissions';

  constructor(private httpClient: HttpClient) { }

  getRoles(
    pageIndex: number,
    pageSize: number,
  ): Observable<PaginationOutput<Role>> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());
    return this.httpClient.get<PaginationOutput<Role>>(this.urlRoles, { params });
  }

  getNewRole(): Observable<RoleOutput> {
    return this.httpClient.get<RoleOutput>(`${this.urlRoles}/new`);
  }

  getRole(id: Guid): Observable<RoleOutput> {
    return this.httpClient.get<RoleOutput>(`${this.urlRoles}/${id}`);
  }

  createRole(role: Role): Observable<Role> {
    return this.httpClient.post<Role>(this.urlRoles, role);
  }

  updateRole(role: Role): Observable<Role> {
    return this.httpClient.put<Role>(`${this.urlRoles}/${role.id}`, role);
  }

  deleteRole(role: Role): Observable<Role> {
    return this.httpClient.delete<Role>(`${this.urlRoles}/${role.id}`);
  }

  getPermissions(
    pageIndex: number,
    pageSize: number,
  ): Observable<PaginationOutput<Permission>> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());
    return this.httpClient.get<PaginationOutput<Permission>>(this.urlPermissions, { params });
  }

  getNewPermission(): Observable<Permission> {
    return this.httpClient.get<Permission>(`${this.urlPermissions}/new`);
  }

  getPermission(id: Guid): Observable<Permission> {
    return this.httpClient.get<Permission>(`${this.urlPermissions}/${id}`);
  }

  createPermission(permission: Permission): Observable<Permission> {
    return this.httpClient.post<Permission>(this.urlPermissions, permission);
  }

  updatePermission(permission: Permission): Observable<Permission> {
    return this.httpClient.put<Permission>(`${this.urlPermissions}/${permission.id}`, permission);
  }

  deletePermission(permission: Permission): Observable<Permission> {
    return this.httpClient.delete<Permission>(`${this.urlPermissions}/${permission.id}`);
  }
}
