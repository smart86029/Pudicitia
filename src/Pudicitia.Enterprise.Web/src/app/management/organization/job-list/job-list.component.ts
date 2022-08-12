import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';

import { Job } from '../job.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.scss'],
})
export class JobListComponent {
  displayedColumns = ['sn', 'title', 'isEnabled', 'employeeCount', 'action'];

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getJobs = (pageEvent: PageEvent) => this.organizationService.getJobs(pageEvent);

  deleteJob(job: Job): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this job (${job.title})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.organizationService.deleteJob(job) : EMPTY),
        tap(() => this.snackBar.open('Deleted')),
      )
      .subscribe();
  }

  canDeleteJob(job: Job): boolean {
    return job.employeeCount === 0;
  }
}
