import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { EMPTY, Subscription } from 'rxjs';
import { finalize, startWith, switchMap, tap } from 'rxjs/operators';

import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { DefaultPaginationOutput, PaginationOutput } from '../../../shared/models/pagination-output.model';
import { IdentityService } from '../identity.service';
import { Permission } from '../permission.model';

@Component({
  selector: 'app-permission-list',
  templateUrl: './permission-list.component.html',
  styleUrls: ['./permission-list.component.scss'],
})
export class PermissionListComponent implements AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  permissions: PaginationOutput<Permission> = new DefaultPaginationOutput<Permission>();
  dataSource = new MatTableDataSource<Permission>();
  displayedColumns = ['rowId', 'code', 'name', 'isEnabled', 'action'];

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  private subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private identityService: IdentityService,
  ) { }

  ngAfterViewInit(): void {
    this.subscription.add(this.paginator.page
      .pipe(
        startWith({}),
        tap(() => this.isLoading = true),
        switchMap(() =>
          this.identityService.getPermissions(
            this.paginator.pageIndex,
            this.paginator.pageSize,
          )),
        tap(permissions => {
          this.isLoading = false;
          this.isEmptyResult = permissions.itemCount === 0;
          this.permissions = permissions;
          this.dataSource.data = permissions.items;
        }),
        finalize(() => this.isLoading = false),
      )
      .subscribe());
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  deletePermission(permission: Permission): void {
    this.dialog
      .open(
        ConfirmDialogComponent,
        { data: `Are you sure to delete this permission (${permission.name})?` },
      )
      .afterClosed()
      .pipe(
        switchMap(result => !!result ? this.identityService.deletePermission(permission) : EMPTY),
        tap(() => {
          this.snackBar.open('Deleted');
          this.paginator._changePageSize(this.paginator.pageSize);
        }),
      )
      .subscribe();
  }
}
