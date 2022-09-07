import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-department-form',
  templateUrl: './department-form.component.html',
  styleUrls: ['./department-form.component.scss'],
})
export class DepartmentFormComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  parent = <Department>{};
  department = <Department>{};
  hasParent = true;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    const parentId = Guid.parse(this.route.snapshot.queryParamMap.get('parentId'));
    let department$ = this.organizationService.getDepartment(parentId);
    let assign = (department: Department) => {
      this.parent = department;
      this.department.parentId = this.parent.id;
    }
    if (Guid.isGuid(id)) {
      this.saveMode = SaveMode.Update;
      this.hasParent = false;
      department$ = this.organizationService.getDepartment(Guid.parse(id));
      assign = (department: Department) => this.department = department;
    }
    department$
      .pipe(
        tap(assign),
        finalize(() => this.isLoading = false),
      )
      .subscribe();
  }

  save(): void {
    let user$ = this.organizationService.createDepartment(this.department);
    if (this.saveMode === SaveMode.Update) {
      user$ = this.organizationService.updateDepartment(this.department);
    }
    user$
      .pipe(
        tap(() => {
          this.snackBar.open(`${SaveMode[this.saveMode]}d`);
          this.back();
        }))
      .subscribe();
  }

  back(): void {
    this.location.back();
  }
}
