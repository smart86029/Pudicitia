import { FlatTreeControl } from '@angular/cdk/tree';
import { AfterContentInit, Component, ContentChildren, Input, OnInit, QueryList, ViewChild } from '@angular/core';
import { MatColumnDef, MatTable } from '@angular/material/table';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { Observable, tap } from 'rxjs';

import { FlatNode } from '../flat-node';

@Component({
  selector: 'app-tree-table',
  templateUrl: './tree-table.component.html',
  styleUrls: ['./tree-table.component.scss'],
})
export class TreeTableComponent<T extends { name: string, children?: T[] }> implements OnInit, AfterContentInit {
  isEmptyResult = false;
  treeControl = new FlatTreeControl<FlatNode<T>>(node => node.level, node => node.expandable);
  treeFlattener = new MatTreeFlattener(
    (value: T, level: number): FlatNode<T> => {
      return {
        expandable: !!value.children && value.children.length > 0,
        name: value.name,
        value: value,
        level: level,
      }
    },
    node => node.level,
    node => node.expandable,
    node => node.children,
  );
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
  headerColumns: string[] = [];
  @Input() displayedColumns!: string[];
  @Input() getItems!: () => Observable<T[]>;

  @ViewChild(MatTable, { static: true }) private table!: MatTable<T>;
  @ContentChildren(MatColumnDef) private columnDefs!: QueryList<MatColumnDef>;

  ngOnInit(): void {
    this.getItems()
      .pipe(
        tap((items: T[]) => {
          this.isEmptyResult = items.length === 0;
          this.dataSource.data = items;
          this.treeControl.expandAll();
        }),
      )
      .subscribe();
    this.headerColumns = ['empty', ...this.displayedColumns];
  }

  ngAfterContentInit() {
    this.columnDefs.forEach(columnDef => this.table.addColumnDef(columnDef));
  }

  hasChild = (_: number, node: FlatNode<T>) => node.expandable;
}
