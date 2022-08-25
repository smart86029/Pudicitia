import { Component } from '@angular/core';
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
export class EmployeeListComponent {
  displayedColumns = ['sn', 'name', 'display-name', 'department', 'job-title', 'action'];
  departments!: Department[];
  name$ = new BehaviorSubject<string | undefined>(undefined);
  department$ = new BehaviorSubject<Department | undefined>(undefined);

  constructor(
    private route: ActivatedRoute,
    private organizationService: OrganizationService,
  ) { }

  getDepartments = () => combineLatest([
    this.organizationService.getDepartments(),
    this.route.queryParamMap,
  ])
    .pipe(
      tap(([departments, queryParamMap]) => {
        const departmentId = Guid.parse(queryParamMap.get('departmentId')!);
        departments.forEach(department => this.setDepartment(department, departmentId));
      }),
      map(([departments]) => departments),
    );

  setDepartment(department: Department, departmentId: Guid): void {
    if (department.id == departmentId) {
      this.department$.next(department);
      return;
    }
    if (department.children) {
      department.children.forEach(child => this.setDepartment(child, departmentId));
    }
  }

  getEmployees = (pageEvent: PageEvent) => combineLatest([
    this.name$,
    this.department$,
  ])
    .pipe(
      switchMap(([name, department]) => this.organizationService.getEmployees(
        pageEvent,
        name,
        department?.id,
      )),
    );
}
