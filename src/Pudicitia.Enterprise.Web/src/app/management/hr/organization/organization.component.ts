import { NestedTreeControl } from '@angular/cdk/tree';
import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { combineLatest, EMPTY, Subscription } from 'rxjs';
import { filter, finalize, startWith, switchMap, tap } from 'rxjs/operators';
import { Guid } from 'src/app/core/guid';
import { PaginationOutput } from 'src/app/core/pagination-output';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

import { Department } from '../department';
import { DepartmentDialogComponent } from '../department-dialog/department-dialog.component';
import { Employee } from '../employee';
import { HRService } from '../hr.service';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
})
export class OrganizationComponent implements OnInit, AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  dataSource = new MatTreeNestedDataSource<Department>();
  treeControl = new NestedTreeControl<Department>(
    department => department.children
  );
  department = new Department();
  employees = new PaginationOutput<Employee>();
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
  paginator: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private hrService: HRService
  ) {}

  ngOnInit(): void {
    this.hrService
      .getDepartments()
      .pipe(
        tap(departments => {
          this.dataSource.data = departments;
          this.treeControl.dataNodes = departments;
          this.treeControl.expandAll();
          if (departments.length > 0) {
            this.department = departments[0];
          }
        })
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
          switchMap(() =>
            this.hrService.getEmployees(
              this.paginator.pageIndex,
              this.paginator.pageSize,
              this.department.id
            )
          ),
          tap(employees => {
            this.isLoading = false;
            this.isEmptyResult = employees.itemCount === 0;
            this.employees = employees;
            this.dataSourceTable.data = employees.items;
          }),
          finalize(() => (this.isLoading = false))
        )
        .subscribe()
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
        switchMap(result =>
          !!result ? this.hrService.createDepartment(result) : EMPTY
        ),
        tap(() => {
          this.snackBar.open('Created');
          this.ngOnInit();
        })
      )
      .subscribe();
  }

  updateDepartment(): void {}

  deleteDepartment(): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: `Are you sure to delete this department (${this.department.name})?`,
      })
      .afterClosed()
      .pipe(
        switchMap(result =>
          !!result ? this.hrService.deleteDepartment(this.department) : EMPTY
        ),
        tap(() => {
          this.snackBar.open('Deleted');
          this.ngOnInit();
        })
      )
      .subscribe();
  }

  canDeleteDepartment(): boolean {
    return !this.department?.children || this.department?.children.length === 0;
  }
}
