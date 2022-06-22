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
import { combineLatest, finalize, Observable, of, startWith, Subscription, switchMap, tap } from 'rxjs';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent<T, U> implements AfterContentInit, AfterViewInit, OnDestroy {
  isLoading = true;
  isEmptyResult = false;
  items: PaginationOutput<T> = new DefaultPaginationOutput<T>();
  tableDataSource = new MatTableDataSource<T>();
  @Input() displayedColumns!: string[];
  @Input() filter$?: Observable<U>;
  @Input() getItems!: (pageEvent: PageEvent) => Observable<PaginationOutput<T>>;

  private subscription = new Subscription();
  @ViewChild(MatTable, { static: true }) private table!: MatTable<T>;
  @ViewChild(MatPaginator) private paginator!: MatPaginator;
  @ContentChildren(MatColumnDef) private columnDefs!: QueryList<MatColumnDef>;

  ngAfterContentInit() {
    this.columnDefs.forEach(columnDef => this.table.addColumnDef(columnDef));
  }

  ngAfterViewInit(): void {
    this.subscription.add(
      combineLatest([
        this.paginator.page.pipe(startWith(<PageEvent>{ pageIndex: 0, pageSize: 0 })),
        this.filter$ || of({}),
      ])
        .pipe(
          tap(() => this.isLoading = true),
          switchMap(([pageEvent]) => this.getItems(pageEvent)),
          tap(items => {
            this.isLoading = false;
            this.isEmptyResult = items.itemCount === 0;
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
