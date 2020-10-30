import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { startWith, switchMap, tap } from 'rxjs/operators';
import {
  DefaultPaginationOutput,
  PaginationOutput,
} from 'src/app/shared/models/pagination-output.model';

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
  permissions: PaginationOutput<Permission> = new DefaultPaginationOutput<
    Permission
  >();
  dataSource = new MatTableDataSource<Permission>();
  displayedColumns = ['rowId', 'code', 'name', 'isEnabled', 'action'];

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  private subscription = new Subscription();

  constructor(private identityService: IdentityService) {}

  ngAfterViewInit(): void {
    this.subscription.add(this.paginator.page
        .pipe(
          startWith({}),
          tap(() => (this.isLoading = true)),
          switchMap(() =>
            this.identityService.getPermissions(
              this.paginator.pageIndex,
              this.paginator.pageSize
            )),
          tap(permissions => {
            this.isLoading = false;
            this.isEmptyResult = permissions.itemCount === 0;
            this.permissions = permissions;
            this.dataSource.data = permissions.items;
          })
        )
        .subscribe());
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
