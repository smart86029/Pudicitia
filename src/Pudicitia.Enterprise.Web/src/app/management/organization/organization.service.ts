import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Department } from './department.model';
import { Employee } from './employee.model';
import { OrganizationOutput } from './organization-output.model';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private urlOrganization = 'api/organization';
  private urlDepartments = 'api/organization/departments';
  private urlEmployees = 'api/organization/employees';

  constructor(private httpClient: HttpClient) { }

  getOrganization(): Observable<OrganizationOutput> {
    return this.httpClient.get<OrganizationOutput>(this.urlOrganization);
  }

  getDepartments(): Observable<Department[]> {
    return this.httpClient.get<Department[]>(`${this.urlDepartments}`);
  }

  getNewDepartment(): Observable<Department> {
    return this.httpClient.get<Department>(`${this.urlDepartments}/new`);
  }

  getDepartment(id: Guid): Observable<Department> {
    return this.httpClient.get<Department>(`${this.urlDepartments}/${id}`);
  }

  createDepartment(department: Department): Observable<Department> {
    return this.httpClient.post<Department>(`${this.urlDepartments}`, department);
  }

  updateDepartment(department: Department): Observable<Department> {
    return this.httpClient.put<Department>(`${this.urlDepartments}/${department.id}`, department);
  }

  deleteDepartment(department: Department): Observable<Department> {
    return this.httpClient.delete<Department>(`${this.urlDepartments}/${department.id}`);
  }

  getEmployees(
    pageIndex: number,
    pageSize: number,
    departmentId: Guid,
  ): Observable<PaginationOutput<Employee>> {
    const params = new HttpParams({
      fromObject: {
        pageIndex,
        pageSize,
        departmentId: departmentId.toString(),
      },
    });
    return this.httpClient.get<PaginationOutput<Employee>>(this.urlEmployees, { params });
  }

  getEmployee(id: Guid): Observable<Employee> {
    return this.httpClient.get<Employee>(`${this.urlEmployees}/${id}`);
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.httpClient.post<Employee>(`${this.urlEmployees}`, employee);
  }

  updateEmployee(employee: Employee): Observable<Employee> {
    return this.httpClient.put<Employee>(`${this.urlEmployees}/${employee.id}`, employee);
  }

  deleteEmployee(employee: Employee): Observable<Employee> {
    return this.httpClient.delete<Employee>(`${this.urlEmployees}/${employee.id}`);
  }
}
