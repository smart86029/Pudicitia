import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { Observable, tap } from 'rxjs';

import { FlatNode } from '../flat-node';

@Component({
  selector: 'app-tree-select',
  templateUrl: './tree-select.component.html',
  styleUrls: ['./tree-select.component.scss'],
})
export class TreeSelectComponent<T extends { name: string, children?: T[] }, U> implements OnInit {
  treeControl = new FlatTreeControl<FlatNode<T>>(node => node.level, node => node.expandable);
  treeFlattener = new MatTreeFlattener(
    (value: T, level: number): FlatNode<T> => {
      return {
        expandable: !!value.children && value.children.length > 0,
        name: value.name,
        value: value,
        level: level,
      };
    },
    node => node.level,
    node => node.expandable,
    node => node.children,
  );
  dataSource = new MatTreeFlatDataSource(
    this.treeControl,
    this.treeFlattener,
  );
  @Input() label?: string;
  @Input() value?: T;
  @Input() getItems!: () => Observable<T[]>;
  @Output() valueChange = new EventEmitter<T>();

  @ViewChild(MatAutocompleteTrigger) private autocompleteTrigger!: MatAutocompleteTrigger;

  ngOnInit(): void {
    this.getItems()
      .pipe(
        tap(items => {
          this.dataSource.data = items;
          this.treeControl.expandAll();
        }),
      )
      .subscribe();
  }

  hasChild = (_: number, node: FlatNode<T>) => node.expandable;

  selectNode(node: FlatNode<T>): void {
    this.value = node.value;
    this.valueChange.next(node.value);
    this.autocompleteTrigger.closePanel();
  }
}
