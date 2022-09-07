import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss'],
})
export class DepartmentListComponent {
  displayedColumns = ['name', 'is-enabled', 'head', 'employee-count', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getDepartments = () => combineLatest([
    this.isEnabled$,
  ])
    .pipe(
      switchMap(([isEnabled]) => this.organizationService.getDepartments(isEnabled)),
    );

  deleteDepartment(department: Department): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this department (${department.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.deleteDepartment(department) : EMPTY),
        tap(() => this.snackBar.open('Deleted')),
      )
      .subscribe();
  }

  canDeleteDepartment(department: Department): boolean {
    return (!department?.children || department?.children.length === 0) &&
      department.employeeCount === 0;
  }
}
