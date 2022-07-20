import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';

import { Permission } from '../../authorization/permission.model';
import { AuthorizationService } from '../authorization.service';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss'],
})
export class PermissionListComponent {
  displayedColumns = ['sn', 'code', 'name', 'isEnabled', 'action'];

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  getPermissions = (pageEvent: PageEvent) => this.authorizationService.getPermissions(pageEvent);

  deletePermission(permission: Permission): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this permission (${permission.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.authorizationService.deletePermission(permission) : EMPTY),
        tap(() => {
          this.snackBar.open('Deleted');
        }),
      )
      .subscribe();
  }
}
