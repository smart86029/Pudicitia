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
import { MatTableDataSource } from '@angular/material/table';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { combineLatest, forkJoin, Subscription, zip } from 'rxjs';
import {
  defaultIfEmpty,
  filter,
  finalize,
  startWith,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs/operators';
import { Guid } from 'src/app/core/guid';
import { PaginationOutput } from 'src/app/core/pagination-output';

import { Department } from '../department';
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

  constructor(private dialog: MatDialog, private hrService: HRService) {}

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
    let i = 0;
    this.subscription.add(
      combineLatest([
        this.paginator.page.pipe(startWith({})),
        this.departmentId.valueChanges.pipe(
          startWith(Guid.empty),
          tap(() => console.log(i++))
        ),
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
            this.dataSource.data = employees.items;
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

  createDepartment(): void {}

  deleteDepartment(department: Department): void {}

  canDeleteDepartment(): boolean {
    return !this.department?.children || this.department?.children.length === 0;
  }
}
