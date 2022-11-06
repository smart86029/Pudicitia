import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, combineLatest, EMPTY, Observable, switchMap, tap } from 'rxjs';
import { ConfirmDialogComponent } from 'shared/components/confirm-dialog/confirm-dialog.component';
import { BooleanFormat } from 'shared/models/boolean-format.enum';
import { PaginationOutput } from 'shared/models/pagination-output.model';

import { AuthorizationService } from '../authorization.service';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
})
export class RoleListComponent {
  isLoading = true;
  displayedColumns = [
    'sn',
    'name',
    'is-enabled',
    'action',
  ];
  booleanFormat = BooleanFormat.Enabled;
  page$ = new BehaviorSubject<PageEvent>({ pageIndex: 0, pageSize: 0 } as PageEvent);
  name$ = new BehaviorSubject<string | undefined>(undefined);
  isEnabled$ = new BehaviorSubject<boolean | undefined>(undefined);
  roles$: Observable<PaginationOutput<Role>> = this.buildRoles();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) {}

  deleteRole(role: Role): void {
    this.dialog
      .open(ConfirmDialogComponent, { data: `Are you sure to delete this role (${role.name})?` })
      .afterClosed()
      .pipe(
        switchMap(result => (result ? this.authorizationService.deleteRole(role) : EMPTY)),
        tap(() => {
          this.snackBar.open('Deleted');
          // this.paginator._changePageSize(this.paginator.pageSize);
        }),
      )
      .subscribe();
  }

  private buildRoles(): Observable<PaginationOutput<Role>> {
    return combineLatest([
      this.page$,
      this.name$,
      this.isEnabled$,
    ]).pipe(
      tap(() => (this.isLoading = true)),
      switchMap(
        ([
          page,
          name,
          isEnabled,
        ]) => this.authorizationService.getRoles(page, name, isEnabled),
      ),
      tap(() => (this.isLoading = false)),
    );
  }
}
