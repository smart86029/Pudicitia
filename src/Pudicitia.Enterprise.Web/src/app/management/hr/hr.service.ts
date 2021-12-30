import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from 'src/app/shared/models/guid.model';
import { PaginationOutput } from 'src/app/shared/models/pagination-output.model';

import { Department } from './department.model';
import { Employee } from './employee.model';
import { OrganizationOutput } from './organization-output.model';

@Injectable({
  providedIn: 'root',
})
export class HRService {
  private urlOrganization = 'api/hr/organization';
  private urlDepartments = 'api/hr/departments';
  private urlEmployees = 'api/hr/employees';

  constructor(private httpClient: HttpClient) { }

  getOrganization(): Observable<OrganizationOutput> {
    return this.httpClient.get<OrganizationOutput>(this.urlOrganization);
  }

  createDepartment(department: Department): Observable<Department> {
    return this.httpClient.post<Department>(`${this.urlDepartments}`, department);
  }

  deleteDepartment(department: Department): Observable<Department> {
    return this.httpClient.delete<Department>(`${this.urlDepartments}/${department.id}`);
  }

  getEmployees(
    pageIndex: number,
    pageSize: number,
    departmentId: Guid,
  ): Observable<PaginationOutput<Employee>> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString())
      .set('departmentId', departmentId.toString());
    return this.httpClient.get<PaginationOutput<Employee>>(this.urlEmployees, { params });
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.httpClient.post<Employee>(`${this.urlEmployees}`, employee);
  }

  deleteEmployee(employee: Employee): Observable<Employee> {
    return this.httpClient.delete<Employee>(`${this.urlEmployees}/${employee.id}`);
  }
}
