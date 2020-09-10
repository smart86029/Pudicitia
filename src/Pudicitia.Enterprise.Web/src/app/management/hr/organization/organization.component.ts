import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeNestedDataSource } from '@angular/material/tree';

import { Department } from '../department';
import { HRService } from '../hr.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
})
export class OrganizationComponent implements OnInit {
  dataSource = new MatTreeNestedDataSource<Department>();
  treeControl = new NestedTreeControl<Department>(
    department => department.children
  );

  constructor(private dialog: MatDialog, private hrService: HRService) {}

  ngOnInit(): void {
    this.hrService
      .getDepartments()
      .pipe(
        tap(departments => {
          this.dataSource.data = departments;
          this.treeControl.dataNodes = departments;
          this.treeControl.expandAll();
        })
      )
      .subscribe();
  }

  hasChild(_: number, department: Department): boolean {
    return !!department.children && department.children.length > 0;
  }

  createDepartment(parent: Department): void {}

  deleteDepartment(department: Department): void {}
}
