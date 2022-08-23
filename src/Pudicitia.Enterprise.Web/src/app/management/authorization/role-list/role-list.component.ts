import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

import { AuthorizationService } from '../authorization.service';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
})
export class RoleListComponent {
  displayedColumns = ['sn', 'name', 'is-enabled', 'action'];
  booleanFormat = BooleanFormat.Enabled;
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  getRoles = (pageEvent: PageEvent) => combineLatest([
    this.name$,
    this.isEnabled$,
  ])
    .pipe(
      switchMap(([name, isEnabled]) => this.authorizationService.getRoles(pageEvent, name, isEnabled)),
    );

  deleteRole(role: Role): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this role (${role.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => result ? this.authorizationService.deleteRole(role) : EMPTY),
        tap(() => {
          this.snackBar.open('Deleted');
          // this.paginator._changePageSize(this.paginator.pageSize);
        }),
      )
      .subscribe();
  }
}
