import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { finalize, startWith, switchMap, tap } from 'rxjs/operators';

import {
  DefaultPaginationOutput,
  PaginationOutput,
} from '../../../shared/models/pagination-output.model';
import { IdentityService } from '../identity.service';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
})
export class RoleListComponent implements AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  roles: PaginationOutput<Role> = new DefaultPaginationOutput<Role>();
  dataSource = new MatTableDataSource<Role>();
  displayedColumns = ['rowId', 'name', 'isEnabled', 'action'];

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  private subscription = new Subscription();

  constructor(private identityService: IdentityService) {}

  ngAfterViewInit(): void {
    this.subscription.add(
      this.paginator.page
        .pipe(
          startWith({}),
          tap(() => (this.isLoading = true)),
          switchMap(() =>
            this.identityService.getRoles(
              this.paginator.pageIndex,
              this.paginator.pageSize
            )
          ),
          tap(roles => {
            this.isLoading = false;
            this.isEmptyResult = roles.itemCount === 0;
            this.roles = roles;
            this.dataSource.data = roles.items;
          }),
          finalize(() => (this.isLoading = false))
        )
        .subscribe()
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
