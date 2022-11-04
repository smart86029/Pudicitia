import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';

import { FlatNode } from '../flat-node';

@Component({
  selector: 'app-tree-select',
  templateUrl: './tree-select.component.html',
  styleUrls: ['./tree-select.component.scss'],
})
export class TreeSelectComponent<TValue extends { name: string, children?: TValue[] }> implements OnChanges {
  @Input() label = '';
  @Input() items: TValue[] = [];
  @Input() value?: TValue;
  @Output() valueChange = new EventEmitter<TValue>();

  treeControl = new FlatTreeControl<FlatNode<TValue>>(node => node.level, node => node.expandable);
  treeFlattener = new MatTreeFlattener(
    (value: TValue, level: number): FlatNode<TValue> => {
      return {
        expandable: !!value.children && value.children.length > 0,
        name: value.name,
        value,
        level,
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

  @ViewChild(MatAutocompleteTrigger) private trigger!: MatAutocompleteTrigger;

  ngOnChanges(changes: SimpleChanges): void {
    const items = changes['items'];
    if (changes['items']) {
      this.dataSource.data = items.currentValue;
      this.treeControl.expandAll();
    }
  }

  hasChild = (_: number, node: FlatNode<TValue>): boolean => node.expandable;

  selectNode(node: FlatNode<TValue>): void {
    this.value = node.value;
    this.valueChange.next(node.value);
    this.trigger.closePanel();
  }
}
