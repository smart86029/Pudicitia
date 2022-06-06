import { NestedTreeControl } from '@angular/cdk/tree';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { combineLatest, EMPTY, filter, finalize, map, startWith, Subscription, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { Guid } from 'shared/models/guid.model';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

import { DepartmentDialogComponent } from './department-dialog/department-dialog.component';
import { Department } from './department.model';
import { EmployeeDialogComponent } from './employee-dialog/employee-dialog.component';
import { Employee } from './employee.model';
import { Job } from './job.model';
import { OrganizationService } from './organization.service';

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
  departmentId = new UntypedFormControl(Guid.empty);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  ngOnInit(): void {
    this.organizationService
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
          switchMap(() => this.organizationService.getEmployees(
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
      .open(DepartmentDialogComponent, { data: { parent: this.department } })
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.createDepartment(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Created');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  updateDepartment(): void {
    this.dialog
      .open(DepartmentDialogComponent, { data: { departmentId: this.department.id } })
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.updateDepartment(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Updated');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  toggleDepartment(): void {
    this.department.isEnabled = !this.department.isEnabled;
    this.organizationService.updateDepartment(this.department)
      .pipe(
        tap(() => {
          this.snackBar.open('Updated');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  deleteDepartment(): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this department (${this.department.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.deleteDepartment(this.department) : EMPTY),
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
        switchMap(result => result ? this.organizationService.createEmployee(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Created');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }

  updateEmployee(employee: Employee): void {
    this.dialog
      .open(EmployeeDialogComponent, {
        data: {
          employeeId: employee.id,
          jobs: this.jobs,
          department: this.department,
        },
      })
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.updateEmployee(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Updated');
          this.ngOnInit();
        }),
      )
      .subscribe();
  }
}
