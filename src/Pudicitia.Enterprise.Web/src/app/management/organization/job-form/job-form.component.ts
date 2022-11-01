import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Job } from '../job.model';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-job-form',
  templateUrl: './job-form.component.html',
  styleUrls: ['./job-form.component.scss'],
})
export class JobFormComponent {
  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.initFormGroup();
  job$: Observable<Job> = this.initJob();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  save(): void {
    const job = this.formGroup.getRawValue() as Job;
    const job$ = this.saveMode === SaveMode.Update
      ? this.organizationService.updateJob(job)
      : this.organizationService.createJob(job);
    job$
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

  private initFormGroup(): FormGroup {
    return this.formBuilder.group({
      id: Guid.empty,
      title: ['', [Validators.required]],
      isEnabled: true,
    });
  }

  private initJob(): Observable<Job> {
    return this.route.paramMap
      .pipe(
        tap(() => this.isLoading = true),
        switchMap(paramMap => {
          const id = paramMap.get('id');
          if (Guid.isGuid(id)) {
            this.saveMode = SaveMode.Update;
            return this.organizationService.getJob(Guid.parse(id));
          }
          return of({ isEnabled: true } as Job);
        }),
        tap(job => {
          this.formGroup.patchValue(job);
          this.isLoading = false;
        }),
      );
  }
}
