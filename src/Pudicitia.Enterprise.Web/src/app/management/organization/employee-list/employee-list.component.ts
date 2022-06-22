import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, forkJoin, map, switchMap, tap, combineLatest } from 'rxjs';
import { Guid } from 'shared/models/guid.model';

import { Department } from '../department.model';
import { Employee } from '../employee.model';
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
    this.route.paramMap
      .pipe(
        tap(paramMap => {
          const departmentId = Guid.parse(paramMap.get('departmentId')!);
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
    if (department.id === departmentId) {
      console.log(4)
      this.department = department;
      return;
    }
    if (department.children) {
      department.children.forEach(child => this.setDepartment(child, departmentId));
    }
  }

  getValue = (item: Department) => item.id;

  getEmployees = (pageEvent: PageEvent) => this.departmentId$
    .pipe(
      switchMap(departmentId => this.organizationService.getEmployees(
        pageEvent.pageIndex,
        pageEvent.pageSize,
        departmentId,
      )),
    );

  changeDepartment(department: Department): void {
    this.departmentId$.next(department.id);
  }

  createEmployee(): void {
    // this.dialog
    //   .open(EmployeeDialogComponent, {
    //     data: {
    //       jobs: this.jobs,
    //       department: this.department,
    //     },
    //   })
    //   .afterClosed()
    //   .pipe(
    //     switchMap(result => result ? this.organizationService.createEmployee(result) : EMPTY),
    //     tap(() => {
    //       this.snackBar.open('Created');
    //       this.ngOnInit();
    //     }),
    //   )
    //   .subscribe();
  }

  updateEmployee(employee: Employee): void {
    console.log(employee);
    // this.dialog
    //   .open(EmployeeDialogComponent, {
    //     data: {
    //       employeeId: employee.id,
    //       jobs: this.jobs,
    //       department: this.department,
    //     },
    //   })
    //   .afterClosed()
    //   .pipe(
    //     switchMap(result => result ? this.organizationService.updateEmployee(result) : EMPTY),
    //     tap(() => {
    //       this.snackBar.open('Updated');
    //       this.ngOnInit();
    //     }),
    //   )
    //   .subscribe();
  }
}
