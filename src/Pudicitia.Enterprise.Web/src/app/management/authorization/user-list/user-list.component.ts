import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, Observable, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { AuthorizationService } from '../authorization.service';
import { User } from '../user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent {
  isLoading = true;
  displayedColumns = ['sn', 'user-name', 'name', 'display-name', 'is-enabled', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  userName$ = new BehaviorSubject<string | undefined>(undefined);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);
  users$: Observable<PaginationOutput<User>> = this.buildUsers();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

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
          // this.paginator._changePageSize(this.paginator.pageSize);
        }),
      )
      .subscribe();
  }

  private buildUsers(): Observable<PaginationOutput<User>> {
    return combineLatest([
      this.page$,
      this.userName$,
      this.name$,
      this.isEnabled$,
    ])
      .pipe(
        tap(() => this.isLoading = true),
        switchMap(([page, userName, name, isEnabled]) => this.authorizationService.getUsers(page, userName, name, isEnabled)),
        tap(() => this.isLoading = false),
      );
  }
}
