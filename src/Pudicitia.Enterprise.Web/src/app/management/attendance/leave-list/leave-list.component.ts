import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { finalize, startWith, Subscription, switchMap, tap } from 'rxjs';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

import { AttendanceService } from '../attendance.service';
import { Leave } from '../leave.model';

@Component({
  selector: 'app-leave-list',
  templateUrl: './leave-list.component.html',
  styleUrls: ['./leave-list.component.scss'],
})
export class LeaveListComponent implements AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  leaves: PaginationOutput<Leave> = new DefaultPaginationOutput<Leave>();
  dataSource = new MatTableDataSource<Leave>();
  displayedColumns = ['rowId', 'type', 'startedOn', 'endedOn', 'approvalStatus', 'action'];

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private attendanceService: AttendanceService,
  ) { }

  ngAfterViewInit(): void {
    this.subscription.add(
      this.paginator.page
        .pipe(
          startWith({}),
          tap(() => this.isLoading = true),
          switchMap(() => this.attendanceService.getLeaves(
            this.paginator.pageIndex,
            this.paginator.pageSize,
          )),
          tap(leaves => {
            this.isLoading = false;
            this.isEmptyResult = leaves.itemCount === 0;
            this.leaves = leaves;
            this.dataSource.data = leaves.items;
          }),
          finalize(() => this.isLoading = false),
        )
        .subscribe(),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
