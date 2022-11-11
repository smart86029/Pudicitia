import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-department-form',
  templateUrl: './department-form.component.html',
  styleUrls: ['./department-form.component.scss'],
})
export class DepartmentFormComponent {
  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.initFormGroup();
  departments$: Observable<[department: Department, parent?: Department]> = this.initDepartments();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) {}

  save(): void {
    const department = this.formGroup.getRawValue() as Department;
    const department$ =
      this.saveMode === SaveMode.Update
        ? this.organizationService.updateDepartment(department)
        : this.organizationService.createDepartment(department);
    department$
      .pipe(
        tap(() => {
          this.snackBar.open(`${SaveMode[this.saveMode]}d`);
          this.back();
        }),
      )
      .subscribe();
  }

  back(): void {
    this.location.back();
  }

  private initFormGroup(): FormGroup {
    return this.formBuilder.group({
      id: Guid.empty,
      parentId: [
        Guid.empty,
        [Validators.required],
      ],
      name: [
        '',
        [Validators.required],
      ],
      isEnabled: true,
    });
  }

  private initDepartments(): Observable<[department: Department, parent?: Department]> {
    return combineLatest([
      this.route.paramMap,
      this.route.queryParamMap,
    ]).pipe(
      tap(() => (this.isLoading = true)),
      switchMap(
        ([
          paramMap,
          queryParamMap,
        ]) => {
          const id = paramMap.get('id');
          const department$ = Guid.isGuid(id)
            ? this.organizationService.getDepartment(Guid.parse(id))
            : of({} as Department);
          this.saveMode = Guid.isGuid(id) ? SaveMode.Update : SaveMode.Create;

          const parentId = queryParamMap.get('parentId');
          const parent$ = Guid.isGuid(parentId)
            ? this.organizationService.getDepartment(Guid.parse(parentId))
            : of(undefined);
          return combineLatest([
            department$,
            parent$,
          ]);
        },
      ),
      startWith([{}] as [department: Department]),
      tap(
        ([
          department,
          parent,
        ]) => {
          if (parent?.id) {
            department.parentId = parent.id;
          }
          this.formGroup.patchValue(department);
          this.isLoading = false;
        },
      ),
    );
  }
}
