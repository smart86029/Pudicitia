import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, map, mergeWith, Observable, Subject, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Department } from '../department.model';
import { Employee } from '../employee.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent {
  isLoading = true;
  displayedColumns = ['sn', 'name', 'display-name', 'department', 'job-title', 'action'];
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  queryDepartmentId$: Observable<Guid | undefined> = this.buildQueryDepartmentId();
  departmentId$ = new Subject<Guid | undefined>();
  departments$: Observable<Department[]> = this.buildDepartments();
  department?: Department;
  employees$: Observable<PaginationOutput<Employee>> = this.buildEmployees();

  constructor(
    private route: ActivatedRoute,
    private organizationService: OrganizationService,
  ) { }

  private buildQueryDepartmentId(): Observable<Guid | undefined> {
    return this.route.queryParamMap
      .pipe(
        map(queryParamMap => {
          const departmentId = queryParamMap.get('departmentId');
          return Guid.isGuid(departmentId) ? Guid.parse(departmentId) : undefined;
        }),
      );
  }

  private buildDepartments(): Observable<Department[]> {
    return combineLatest([
      this.organizationService.getDepartments(),
      this.queryDepartmentId$,
    ])
      .pipe(
        tap(([departments, departmentId]) => {
          if (departmentId) {
            departments.forEach(department => this.setDepartment(department, departmentId));
          }
        }),
        map(([departments]) => departments),
      );
  }

  private buildEmployees(): Observable<PaginationOutput<Employee>> {
    const departmentId$ = this.queryDepartmentId$.pipe(mergeWith(this.departmentId$));
    return combineLatest([
      this.page$,
      this.name$,
      departmentId$,
    ])
      .pipe(
        tap(() => this.isLoading = true),
        switchMap(([page, name, departmentId]) => this.organizationService.getEmployees(
          page,
          name,
          departmentId,
        )),
        tap(() => this.isLoading = false),
      );
  }

  private setDepartment(department: Department, departmentId: Guid): void {
    if (department.id == departmentId) {
      this.department = department;
      return;
    }
    if (department.children) {
      department.children.forEach(child => this.setDepartment(child, departmentId));
    }
  }
}
