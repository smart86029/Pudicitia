import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';
import { HRService } from '../hr.service';

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

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: { parent: Department, departmentId: Guid },
    private hrService: HRService,
  ) { }

  ngOnInit(): void {
    if (this.data.departmentId) {
      this.saveMode = SaveMode.Update;
      this.hrService.getDepartment(this.data.departmentId)
        .pipe(
          tap(department => this.department = department),
        )
        .subscribe();
    } else {
      this.hasParent = true;
      this.parent = this.data.parent;
      this.department.parentId = this.data.parent.id;
    }
  }
}
