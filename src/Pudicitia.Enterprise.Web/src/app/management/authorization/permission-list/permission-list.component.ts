import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, Observable, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { Permission } from '../../authorization/permission.model';
import { AuthorizationService } from '../authorization.service';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss'],
})
export class PermissionListComponent {
  isLoading = true;
  displayedColumns = ['sn', 'code', 'name', 'is-enabled', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  code$ = new BehaviorSubject<string | undefined>(undefined);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);
  permissions$: Observable<PaginationOutput<Permission>> = this.buildPermissions();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

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

  private buildPermissions(): Observable<PaginationOutput<Permission>> {
    return combineLatest([
      this.page$,
      this.code$,
      this.name$,
      this.isEnabled$,
    ])
      .pipe(
        tap(() => this.isLoading = true),
        switchMap(([page, code, name, isEnabled]) => this.authorizationService.getPermissions(page, code, name, isEnabled)),
        tap(() => this.isLoading = false),
      );
  }
}
