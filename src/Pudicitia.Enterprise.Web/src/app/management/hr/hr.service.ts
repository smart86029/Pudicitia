import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Guid } from 'src/app/core/guid';

import { ListOutput } from '../../core/list-output';
import { Department } from './department';

@Injectable({
  providedIn: 'root',
})
export class HRService {
  private departmentsUrl = 'api/hr/departments';

  constructor(private httpClient: HttpClient) {}

  getDepartments(): Observable<Department[]> {
    return this.httpClient
      .get<ListOutput<Department>>(this.departmentsUrl)
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
      `${this.departmentsUrl}`,
      department
    );
  }

  deleteDepartment(department: Department): Observable<Department> {
    return this.httpClient.delete<Department>(
      `${this.departmentsUrl}/${department.id}`
    );
  }
}
