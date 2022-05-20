import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';

@Component({
  selector: 'app-department-dialog',
  templateUrl: './department-dialog.component.html',
  styleUrls: ['./department-dialog.component.scss'],
})
export class DepartmentDialogComponent implements OnInit {
  saveMode = SaveMode.Create;
  parent = <Department>{};
  department = <Department>{};
  hasParent = false;

  constructor(@Inject(MAT_DIALOG_DATA) private data: { parent: Department, department: Department }) { }

  ngOnInit(): void {
    if (this.data.department) {
      this.saveMode = SaveMode.Update;
      Object.assign(this.department, this.data.department);
    } else {
      this.hasParent = true;
      this.parent = this.data.parent;
      this.department.parentId = this.data.parent.id;
    }
  }
}
