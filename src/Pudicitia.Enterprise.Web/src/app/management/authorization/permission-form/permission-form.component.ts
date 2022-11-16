import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Observable, startWith, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { AuthorizationService } from '../authorization.service';
import { Permission } from '../permission.model';

@Component({
  selector: 'app-permission-form',
  templateUrl: './permission-form.component.html',
  styleUrls: ['./permission-form.component.scss'],
})
export class PermissionFormComponent {
  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.initFormGroup();
  permission$: Observable<Permission> = this.initPermission();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) {}

  save(): void {
    const permission = this.formGroup.getRawValue() as Permission;
    const permission$ =
      this.saveMode === SaveMode.Update
        ? this.authorizationService.updatePermission(permission)
        : this.authorizationService.createPermission(permission);
    permission$
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
      code: [
        '',
        [
          Validators.required,
          Validators.minLength(1),
        ],
      ],
      name: [
        '',
        [Validators.required],
      ],
      description: '',
      isEnabled: true,
    });
  }

  private initPermission(): Observable<Permission> {
    return this.route.paramMap.pipe(
      tap(() => (this.isLoading = true)),
      switchMap(paramMap => {
        const id = paramMap.get('id');
        if (Guid.isGuid(id)) {
          this.saveMode = SaveMode.Update;
          return this.authorizationService.getPermission(Guid.parse(id!));
        }
        return this.authorizationService.getNewPermission();
      }),
      startWith({} as Permission),
      tap(permission => {
        this.formGroup.patchValue(permission);
        this.isLoading = false;
      }),
    );
  }
}
