import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

import { AuthorizationService } from '../authorization.service';
import { User } from '../user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent {
  displayedColumns = ['sn', 'user-name', 'name', 'display-name', 'is-enabled', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  userName$ = new BehaviorSubject<string | undefined>(undefined);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  getUsers = (pageEvent: PageEvent) => combineLatest([
    this.userName$,
    this.name$,
    this.isEnabled$,
  ])
    .pipe(
      switchMap(([userName, name, isEnabled]) => this.authorizationService.getUsers(pageEvent, userName, name, isEnabled)),
    );

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
}
