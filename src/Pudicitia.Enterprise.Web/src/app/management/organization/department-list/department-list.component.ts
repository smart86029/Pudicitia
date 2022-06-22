import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { Guid } from 'shared/models/guid.model';

import { DepartmentDialogComponent } from '../department-dialog/department-dialog.component';
import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss'],
})
export class DepartmentListComponent {
  displayedColumns = ['name', 'head', 'employeeCount', 'action'];
  departments = new Map<Guid, Department>();
  department = <Department>{};

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getDepartments = () => this.organizationService.getDepartments();

  selectDepartment(department: Department): void {
    this.department = department;
  }

  createDepartment(): void {
    this.dialog
      .open(
        DepartmentDialogComponent,
        { data: { parent: this.department } },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.createDepartment(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Created');
        }),
      )
      .subscribe();
  }

  updateDepartment(department: Department): void {
    this.dialog
      .open(
        DepartmentDialogComponent,
        { data: { departmentId: department.id } },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.updateDepartment(result) : EMPTY),
        tap(() => {
          this.snackBar.open('Updated');
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
        }),
      )
      .subscribe();
  }

  canDeleteDepartment(): boolean {
    return !this.department?.children || this.department?.children.length === 0;
  }
}
