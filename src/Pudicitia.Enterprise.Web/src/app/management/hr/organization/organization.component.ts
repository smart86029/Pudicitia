import { NestedTreeControl } from '@angular/cdk/tree';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { combineLatest, EMPTY, filter, finalize, map, startWith, Subscription, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { Guid } from 'shared/models/guid.model';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

import { DepartmentDialogComponent } from '../department-dialog/department-dialog.component';
import { Department } from '../department.model';
import { EmployeeDialogComponent } from '../employee-dialog/employee-dialog.component';
import { Employee } from '../employee.model';
import { HRService } from '../hr.service';
import { Job } from '../job.model';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
})
export class OrganizationComponent implements OnInit, AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  dataSource = new MatTreeNestedDataSource<Department>();
  treeControl = new NestedTreeControl<Department>(department => department.children);
  departments = new Map<Guid, Department>();
  jobs = new Map<Guid, Job>();
  department = <Department>{};
  employees: PaginationOutput<Employee> = new DefaultPaginationOutput<Employee>();
  dataSourceTable = new MatTableDataSource<Employee>();
  displayedColumns = [
    'rowId',
    'name',
    'displayName',
    'department',
    'jobTitle',
    'action',
  ];
  departmentId = new FormControl(Guid.empty);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private hrService: HRService,
  ) { }

  ngOnInit(): void {
    this.hrService
      .getOrganization()
      .pipe(
        tap(output => {
          output.departments.forEach(department => {
            if (!department.children) {
              department.children = [];
            }
          });
          this.departments.clear();
          output.departments.forEach(department => this.departments.set(department.id, department));
          output.jobs.forEach(job => this.jobs.set(job.id, job));
        }),
        map(output => {
          const result: Department[] = [];
          output.departments.forEach(department => {
            if (department.parentId) {
              this.departments.get(department.parentId)!.children!.push(department);
            } else {
              result.push(department);
            }
          });
          return result;
        }),
        tap(departments => {
          this.dataSource.data = departments;
          this.treeControl.dataNodes = departments;
          this.treeControl.expandAll();
          if (departments.length > 0) {
            this.department = departments[0];
          }
        }),
      )
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.subscription.add(
      combineLatest([
        this.paginator.page.pipe(startWith({})),
        this.departmentId.valueChanges.pipe(startWith(Guid.empty)),
      ])
        .pipe(
          filter(() => !!this.department.id),
          tap(() => (this.isLoading = true)),
          switchMap(() => this.hrService.getEmployees(
            this.paginator.pageIndex,
            this.paginator.pageSize,
            this.department.id,
          )),
          tap(employees => {
            this.isLoading = false;
            this.isEmptyResult = employees.itemCount === 0;
            this.employees = employees;
            this.dataSourceTable.data = employees.items;
          }),
          finalize(() => (this.isLoading = false)),
        )
        .subscribe(),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  hasChild(_: number, department: Department): boolean {
    return !!department.children && department.children.length > 0;
  }

  selectDepartment(department: Department): void {
    this.department = department;
  }

  createDepartment(): void {
    this.dialog
      .open(DepartmentDialogComponent, { data: this.department })
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.hrService.createDepartment(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Created');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  updateDepartment(): void { }

  deleteDepartment(): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this department (${this.department.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.hrService.deleteDepartment(this.department) : EMPTY),
        tap(() => {
          this.snackBar.open('Deleted');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  canDeleteDepartment(): boolean {
    return !this.department?.children || this.department?.children.length === 0;
  }

  createEmployee(): void {
    this.dialog
      .open(EmployeeDialogComponent, {
        data: {
          jobs: this.jobs,
          department: this.department,
        },
      })
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.hrService.createEmployee(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Created');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }
}
