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
export class TreeSelectComponent<TValue extends { name: string, children?: TValue[] }> implements OnInit {
  treeControl = new FlatTreeControl<FlatNode<TValue>>(node => node.level, node => node.expandable);
  treeFlattener = new MatTreeFlattener(
    (value: TValue, level: number): FlatNode<TValue> => {
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
  @Input() value?: TValue;
  @Input() getItems!: () => Observable<TValue[]>;
  @Output() valueChange = new EventEmitter<TValue>();

  @ViewChild(MatAutocompleteTrigger) private trigger!: MatAutocompleteTrigger;

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

  hasChild = (_: number, node: FlatNode<TValue>) => node.expandable;

  selectNode(node: FlatNode<TValue>): void {
    this.value = node.value;
    this.valueChange.next(node.value);
    this.trigger.closePanel();
  }
}
