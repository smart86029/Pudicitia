import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Permission } from './permission.model';
import { RoleOutput } from './role-output.model';
import { Role } from './role.model';
import { UserOutput } from './user-output.model';
import { User } from './user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationService {
  private urlUsers = 'api/authorization/users';
  private urlRoles = 'api/authorization/roles';
  private urlPermissions = 'api/authorization/permissions';

  constructor(private httpClient: HttpClient) { }

  getUsers(
    page: { pageIndex: number, pageSize: number },
    userName?: string,
    name?: string,
    isEnabled?: boolean,
  ): Observable<PaginationOutput<User>> {
    let params = new HttpParams({
      fromObject: page,
    });
    if (userName !== undefined) {
      params = params.set('userName', userName!);
    }
    if (name !== undefined) {
      params = params.set('name', name!);
    }
    if (isEnabled !== undefined) {
      params = params.set('isEnabled', isEnabled!);
    }
    return this.httpClient.get<PaginationOutput<User>>(this.urlUsers, { params });
  }

  getNewUser(): Observable<UserOutput> {
    return this.httpClient.get<UserOutput>(`${this.urlUsers}/new`);
  }

  getUser(id: Guid): Observable<UserOutput> {
    return this.httpClient.get<UserOutput>(`${this.urlUsers}/${id}`);
  }

  createUser(user: User): Observable<User> {
    return this.httpClient.post<User>(this.urlUsers, user);
  }

  updateUser(user: User): Observable<User> {
    return this.httpClient.put<User>(`${this.urlUsers}/${user.id}`, user);
  }

  deleteUser(user: User): Observable<User> {
    return this.httpClient.delete<User>(`${this.urlUsers}/${user.id}`);
  }

  getRoles(
    page: { pageIndex: number, pageSize: number },
    name?: string,
    isEnabled?: boolean,
  ): Observable<PaginationOutput<Role>> {
    let params = new HttpParams({
      fromObject: page,
    });
    if (name !== undefined) {
      params = params.set('name', name!);
    }
    if (isEnabled !== undefined) {
      params = params.set('isEnabled', isEnabled!);
    }
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
    page: { pageIndex: number, pageSize: number },
    code?: string,
    name?: string,
    isEnabled?: boolean,
  ): Observable<PaginationOutput<Permission>> {
    let params = new HttpParams({
      fromObject: page,
    });
    if (code !== undefined) {
      params = params.set('code', code!);
    }
    if (name !== undefined) {
      params = params.set('name', name!);
    }
    if (isEnabled !== undefined) {
      params = params.set('isEnabled', isEnabled!);
    }
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
