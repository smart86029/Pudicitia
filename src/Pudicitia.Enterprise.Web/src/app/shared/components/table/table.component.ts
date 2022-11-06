import {
  AfterContentInit,
  Component,
  ContentChildren,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatColumnDef, MatTable, MatTableDataSource } from '@angular/material/table';
import { DefaultPaginationOutput, PaginationOutput } from 'shared/models/pagination-output.model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent<TItem> implements OnChanges, AfterContentInit {
  @Input() isLoading = false;
  @Input() displayedColumns: string[] = [];
  @Input() items: PaginationOutput<TItem> = new DefaultPaginationOutput<TItem>();
  @Output() readonly page = new EventEmitter<PageEvent>();

  isEmptyResult = false;
  dataSource = new MatTableDataSource<TItem>();

  @ViewChild(MatTable, { static: true }) private table!: MatTable<TItem>;
  @ContentChildren(MatColumnDef) private columnDefs!: QueryList<MatColumnDef>;

  ngOnChanges(changes: SimpleChanges): void {
    const items = changes['items'];
    if (items) {
      this.isEmptyResult = items.currentValue.page.itemCount === 0;
      this.dataSource.data = items.currentValue.items;
    }
  }

  ngAfterContentInit(): void {
    this.columnDefs.forEach(columnDef => this.table.addColumnDef(columnDef));
  }
}
