import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Guid } from 'src/app/core/guid';
import { ListOutput } from 'src/app/core/list-output';
import { PaginationOutput } from 'src/app/core/pagination-output';

import { Department } from './department';
import { Employee } from './employee';

@Injectable({
  providedIn: 'root',
})
export class HRService {
  private urlDepartments = 'api/hr/departments';
  private urlEmployees = 'api/hr/employees';

  constructor(private httpClient: HttpClient) {}

  getDepartments(): Observable<Department[]> {
    return this.httpClient
      .get<ListOutput<Department>>(this.urlDepartments)
      .pipe(
        tap(output => {
          output.items.forEach(department => {
            if (!department.children) {
              department.children = [];
            }
          });
        }),
        map(output => {
          const treeMap = new Map<Guid, Department>();
          const result: Department[] = [];
          output.items.forEach(department =>
            treeMap.set(department.id, department)
          );
          output.items.forEach(department => {
            if (!!department.parentId) {
              treeMap.get(department.parentId).children.push(department);
            } else {
              result.push(department);
            }
          });
          return result;
        })
      );
  }

  createDepartment(department: Department): Observable<Department> {
    return this.httpClient.post<Department>(
      `${this.urlDepartments}`,
      department
    );
  }

  deleteDepartment(department: Department): Observable<Department> {
    return this.httpClient.delete<Department>(
      `${this.urlDepartments}/${department.id}`
    );
  }

  getEmployees(
    pageIndex: number,
    pageSize: number,
    departmentId: Guid
  ): Observable<PaginationOutput<Employee>> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString())
      .set('departmentId', departmentId.toString());
    return this.httpClient.get<PaginationOutput<Employee>>(this.urlEmployees, {
      params,
    });
  }
}
