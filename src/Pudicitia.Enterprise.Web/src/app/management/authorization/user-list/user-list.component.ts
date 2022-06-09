import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { EMPTY, finalize, startWith, Subscription, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

import { AuthorizationService } from '../authorization.service';
import { User } from '../user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  users: PaginationOutput<User> = new DefaultPaginationOutput<User>();
  dataSource = new MatTableDataSource<User>();
  displayedColumns = ['rowId', 'userName', 'name', 'displayName', 'isEnabled', 'action'];

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  ngAfterViewInit(): void {
    this.subscription.add(
      this.paginator.page
        .pipe(
          startWith({}),
          tap(() => this.isLoading = true),
          switchMap(() => this.authorizationService.getUsers(
            this.paginator.pageIndex,
            this.paginator.pageSize,
          )),
          tap(users => {
            this.isLoading = false;
            this.isEmptyResult = users.itemCount === 0;
            this.users = users;
            this.dataSource.data = users.items;
          }),
          finalize(() => this.isLoading = false),
        )
        .subscribe(),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  deleteUser(user: User): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this user (${user.userName})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.authorizationService.deleteUser(user) : EMPTY),
        tap(() => {
          this.snackBar.open('Deleted');
          this.paginator._changePageSize(this.paginator.pageSize);
        }),
      )
      .subscribe();
  }
}
