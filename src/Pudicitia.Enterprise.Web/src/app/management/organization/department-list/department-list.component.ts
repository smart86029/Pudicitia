import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { Guid } from 'shared/models/guid.model';

import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss'],
})
export class DepartmentListComponent {
  displayedColumns = ['name', 'head', 'employee-count', 'action'];
  departments = new Map<Guid, Department>();
  department = <Department>{};

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getDepartments = () => this.organizationService.getDepartments();

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
    return !department?.children || department?.children.length === 0;
  }
}
