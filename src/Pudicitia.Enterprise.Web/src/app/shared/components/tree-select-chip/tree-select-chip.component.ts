import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { FlatNode } from '../flat-node';

@Component({
  selector: 'app-tree-select-chip',
  templateUrl: './tree-select-chip.component.html',
  styleUrls: ['./tree-select-chip.component.scss'],
})
export class TreeSelectChipComponent<TValue extends { name: string, children?: TValue[] }> implements OnInit {
  hasValue = false;
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
  @Input() getItems!: () => Observable<TValue[]>;
  @Input() value$ = new BehaviorSubject<TValue | undefined>(undefined);

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
    this.value$
      .pipe(
        tap(value => this.hasValue = value !== undefined),
      )
      .subscribe();
  }

  hasChild = (_: number, node: FlatNode<TValue>) => node.expandable;

  selectNode(node: FlatNode<TValue>): void {
    this.value$.next(node.value);
    this.trigger.closePanel();
  }
}
