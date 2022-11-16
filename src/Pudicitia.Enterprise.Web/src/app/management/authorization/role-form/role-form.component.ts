import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Observable, startWith, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { AuthorizationService } from '../authorization.service';
import { RoleOutput } from '../role-output.model';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.scss'],
})
export class RoleFormComponent {
  isLoading = true;
  saveMode = SaveMode.Update;
  formGroup: FormGroup = this.initFormGroup();
  roleOutput$: Observable<RoleOutput> = this.initRoleOutput();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) {}

  save(): void {
    const role = this.formGroup.getRawValue() as Role;
    const role$ =
      this.saveMode === SaveMode.Update
        ? this.authorizationService.updateRole(role)
        : this.authorizationService.createRole(role);
    role$
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
      name: [
        '',
        [Validators.required],
      ],
      isEnabled: true,
      permissionIds: [] as Guid[],
    });
  }

  private initRoleOutput(): Observable<RoleOutput> {
    return this.route.paramMap.pipe(
      tap(() => (this.isLoading = true)),
      switchMap(paramMap => {
        const id = paramMap.get('id');
        if (Guid.isGuid(id)) {
          this.saveMode === SaveMode.Update;
          return this.authorizationService.getRole(Guid.parse(id));
        }
        return this.authorizationService.getNewRole();
      }),
      startWith({} as RoleOutput),
      tap(output => {
        this.formGroup.patchValue(output.role);
        this.isLoading = false;
      }),
    );
  }
}
