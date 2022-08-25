import {
  AfterContentInit,
  AfterViewInit,
  Component,
  ContentChildren,
  Input,
  OnDestroy,
  QueryList,
  ViewChild,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatColumnDef, MatTable, MatTableDataSource } from '@angular/material/table';
import { finalize, Observable, startWith, Subscription, switchMap, tap } from 'rxjs';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent<TItem> implements AfterContentInit, AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  items: PaginationOutput<TItem> = new DefaultPaginationOutput<TItem>();
  tableDataSource = new MatTableDataSource<TItem>();
  @Input() displayedColumns!: string[];
  @Input() getItems!: (pageEvent: PageEvent) => Observable<PaginationOutput<TItem>>;

  private subscription = new Subscription();
  @ViewChild(MatTable, { static: true }) private table!: MatTable<TItem>;
  @ViewChild(MatPaginator) private paginator!: MatPaginator;
  @ContentChildren(MatColumnDef) private columnDefs!: QueryList<MatColumnDef>;

  ngAfterContentInit(): void {
    this.columnDefs.forEach(columnDef => this.table.addColumnDef(columnDef));
  }

  ngAfterViewInit(): void {
    this.subscription.add(
      this.paginator.page
        .pipe(
          startWith(<PageEvent>{ pageIndex: 0, pageSize: 0 }),
          tap(() => this.isLoading = true),
          switchMap(pageEvent => this.getItems(pageEvent)),
          tap(items => {
            this.isLoading = false;
            this.isEmptyResult = items.page.itemCount === 0;
            this.items = items;
            this.tableDataSource.data = items.items;
          }),
          finalize(() => this.isLoading = false),
        )
        .subscribe(),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
