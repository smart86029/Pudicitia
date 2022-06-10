import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';

import { AuthorizationService } from '../authorization.service';
import { User } from '../user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent {
  displayedColumns = ['sn', 'userName', 'name', 'displayName', 'isEnabled', 'action'];

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  getUsers = (pageEvent: PageEvent) => this.authorizationService.getUsers(pageEvent.pageIndex, pageEvent.pageSize);

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
