import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Department } from './department.model';
import { EmployeeOutput } from './employee-output.model';
import { Employee } from './employee.model';
import { Job } from './job.model';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  private urlDepartments = 'api/organization/departments';
  private urlEmployees = 'api/organization/employees';
  private urlJobs = 'api/organization/jobs';

  constructor(private httpClient: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    return this.httpClient.get<Department[]>(`${this.urlDepartments}`)
      .pipe(
        map(departments => this.getDepartmentTree(departments)),
      );
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
    page: { pageIndex: number, pageSize: number },
    name?: string,
    departmentId?: Guid,
  ): Observable<PaginationOutput<Employee>> {
    let params = new HttpParams({
      fromObject: page,
    });
    if (name !== undefined) {
      params = params.set('name', name);
    }
    if (departmentId !== undefined) {
      params = params.set('departmentId', departmentId.toString());
    }
    return this.httpClient.get<PaginationOutput<Employee>>(this.urlEmployees, { params });
  }

  getNewEmployee(): Observable<EmployeeOutput> {
    return this.httpClient.get<EmployeeOutput>(`${this.urlEmployees}/new`)
      .pipe(
        tap(output => output.departments = this.getDepartmentTree(output.departments)),
      );
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

  getJobs(
    page: { pageIndex: number, pageSize: number },
    title?: string,
    isEnabled?: boolean,
  ): Observable<PaginationOutput<Job>> {
    let params = new HttpParams({
      fromObject: page,
    });
    if (title !== undefined) {
      params = params.set('title', title);
    }
    if (isEnabled !== undefined) {
      params = params.set('isEnabled', isEnabled);
    }
    return this.httpClient.get<PaginationOutput<Job>>(this.urlJobs, { params });
  }

  getJob(id: Guid): Observable<Job> {
    return this.httpClient.get<Job>(`${this.urlJobs}/${id}`);
  }

  createJob(job: Job): Observable<Job> {
    return this.httpClient.post<Job>(`${this.urlJobs}`, job);
  }

  updateJob(job: Job): Observable<Job> {
    return this.httpClient.put<Job>(`${this.urlJobs}/${job.id}`, job);
  }

  deleteJob(job: Job): Observable<Job> {
    return this.httpClient.delete<Job>(`${this.urlJobs}/${job.id}`);
  }

  private getDepartmentTree(departments: Department[]): Department[] {
    const dictionary = new Map<Guid, Department>();
    const result: Department[] = [];
    departments.forEach(department => {
      if (!department.children) {
        department.children = [];
      }
    });
    departments.forEach(department => dictionary.set(department.id, department));
    departments.forEach(department => {
      if (department.parentId) {
        dictionary.get(department.parentId)!.children!.push(department);
      } else {
        result.push(department);
      }
    });
    return result;
  }
}
