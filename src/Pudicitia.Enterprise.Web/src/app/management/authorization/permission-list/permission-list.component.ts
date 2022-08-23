import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

import { Permission } from '../../authorization/permission.model';
import { AuthorizationService } from '../authorization.service';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss'],
})
export class PermissionListComponent {
  displayedColumns = ['sn', 'code', 'name', 'is-enabled', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  code$ = new BehaviorSubject<string | undefined>(undefined);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  getPermissions = (pageEvent: PageEvent) => combineLatest([
    this.code$,
    this.name$,
    this.isEnabled$,
  ])
    .pipe(
      switchMap(([code, name, isEnabled]) => this.authorizationService.getPermissions(pageEvent, code, name, isEnabled)),
    );

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
