import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Observable, startWith, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { AuthorizationService } from '../authorization.service';
import { UserOutput } from '../user-output.model';
import { User } from '../user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent {
  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.initFormGroup();
  userOutput$: Observable<UserOutput> = this.initUserOutput();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private authorizationService: AuthorizationService,
  ) { }

  save(): void {
    const user = this.formGroup.getRawValue() as User;
    const user$ = this.saveMode === SaveMode.Update
      ? this.authorizationService.updateUser(user)
      : this.authorizationService.createUser(user);
    user$
      .pipe(
        tap(() => {
          this.snackBar.open(`${this.saveMode}d`);
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
      id: [''],
      userName: ['', [Validators.required]],
      password: [''],
      name: ['', [Validators.required]],
      displayName: ['', [Validators.required]],
      isEnabled: [true],
      roleIds: [''],
    });
  }

  private initUserOutput(): Observable<UserOutput> {
    return this.route.params.pipe(
      tap(() => this.isLoading = true),
      switchMap(params => {
        const id = params['id'];
        if (Guid.isGuid(id)) {
          this.saveMode = SaveMode.Update;
          return this.authorizationService.getUser(Guid.parse(id!));
        }
        return this.authorizationService.getNewUser();
      }),
      startWith({} as UserOutput),
      tap(output => {
        this.formGroup.patchValue(output.user);
        this.isLoading = false;
      }),
    );
  }
}
