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
  department = <Department>{};

  constructor(@Inject(MAT_DIALOG_DATA) public parent: Department) { }

  ngOnInit(): void {
    this.department.parentId = this.parent.id;
  }
}
