import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, Observable, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Job } from '../job.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.scss'],
})
export class JobListComponent {
  isLoading = true;
  displayedColumns = ['sn', 'title', 'is-enabled', 'employee-count', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  title$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);
  jobs$: Observable<PaginationOutput<Job>> = this.buildJobs();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) {}

  deleteJob(job: Job): void {
    this.dialog
      .open(ConfirmDialogComponent, { data: `Are you sure to delete this job (${job.title})?` })
      .afterClosed()
      .pipe(
        switchMap(result => (result ? this.organizationService.deleteJob(job) : EMPTY)),
        tap(() => this.snackBar.open('Deleted')),
      )
      .subscribe();
  }

  canDeleteJob(job: Job): boolean {
    return job.employeeCount === 0;
  }

  private buildJobs(): Observable<PaginationOutput<Job>> {
    return combineLatest([this.page$, this.title$, this.isEnabled$]).pipe(
      tap(() => (this.isLoading = true)),
      switchMap(([page, title, isEnabled]) => this.organizationService.getJobs(page, title, isEnabled)),
      tap(() => (this.isLoading = false)),
    );
  }
}
