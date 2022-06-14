import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Guid } from 'shared/models/guid.model';

import { Employee } from '../employee.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss'],
})
export class EmployeeListComponent {
  displayedColumns = ['sn', 'name', 'displayName', 'department', 'jobTitle', 'action'];

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getEmployees = (pageEvent: PageEvent) => this.organizationService.getEmployees(pageEvent.pageIndex, pageEvent.pageSize, Guid.empty);

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
