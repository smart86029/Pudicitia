import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, startWith, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../../../core/attendance/leave-type.enum';
import { Leave } from '../leave.model';

@Component({
  selector: 'app-leave-form',
  templateUrl: './leave-form.component.html',
  styleUrls: ['./leave-form.component.scss'],
})
export class LeaveFormComponent {
  LeaveType = LeaveType;
  ApprovalStatus = ApprovalStatus;

  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.buildFormGroup();
  leave$: Observable<Leave> = this.buildLeave();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private attendanceService: AttendanceService,
  ) {}

  private buildFormGroup(): FormGroup {
    return this.formBuilder.group({
      id: Guid.empty,
      title: [
        '',
        [Validators.required],
      ],
      isEnabled: true,
    });
  }

  private buildLeave(): Observable<Leave> {
    return this.route.paramMap.pipe(
      tap(() => (this.isLoading = true)),
      switchMap(paramMap => {
        const id = paramMap.get('id');
        if (Guid.isGuid(id)) {
          this.saveMode = SaveMode.Update;
          return this.attendanceService.getLeave(Guid.parse(id));
        }
        return of({} as Leave);
      }),
      startWith({} as Leave),
      tap(job => {
        this.formGroup.patchValue(job);
        this.isLoading = false;
      }),
    );
  }
}
