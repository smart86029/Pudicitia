import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, map, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';

import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent implements OnInit {
  displayedColumns = ['sn', 'name', 'displayName', 'department', 'jobTitle', 'action'];
  departments!: Department[];
  department?: Department;
  departmentId$ = new BehaviorSubject<Guid>(Guid.empty);

  constructor(
    private route: ActivatedRoute,
    private organizationService: OrganizationService,
  ) { }

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(
        tap(queryParamMap => {
          const departmentId = Guid.parse(queryParamMap.get('departmentId')!);
          this.departmentId$.next(departmentId);
        }),
      )
      .subscribe();
  }

  getDepartments = () => combineLatest([
    this.organizationService.getDepartments(),
    this.departmentId$,
  ])
    .pipe(
      tap(([departments, departmentId]) => {
        departments.forEach(department => this.setDepartment(department, departmentId));
      }),
      map(([departments]) => departments),
    );

  setDepartment(department: Department, departmentId: Guid): void {
    if (department.id == departmentId) {
      this.department = department;
      return;
    }
    if (department.children) {
      department.children.forEach(child => this.setDepartment(child, departmentId));
    }
  }

  getEmployees = (pageEvent: PageEvent) => this.departmentId$
    .pipe(
      switchMap(departmentId => this.organizationService.getEmployees(
        pageEvent,
        departmentId,
      )),
    );

  changeDepartment(department: Department): void {
    this.departmentId$.next(department.id);
  }
}
