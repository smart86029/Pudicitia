import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LeaveType } from 'core/attendance/leave-type.enum';
import { Guid } from 'shared/models/guid.model';

import { EventDialogParam } from './event-dialog-param.model';

@Component({
  selector: 'app-event-dialog',
  templateUrl: './event-dialog.component.html',
  styleUrls: ['./event-dialog.component.scss'],
})
export class EventDialogComponent {
  LeaveType = LeaveType;

  formGroup: FormGroup = this.initFormGroup();

  constructor(
    @Inject(MAT_DIALOG_DATA) public param: EventDialogParam,
    private dialogRef: MatDialogRef<EventDialogComponent>,
    private formBuilder: FormBuilder,
  ) {}

  save(): void {
    this.dialogRef.close();
  }

  private initFormGroup(): FormGroup {
    return this.formBuilder.group({
      id: Guid.empty,
      leaveType: [
        LeaveType.Other,
        [Validators.required],
      ],
      startedOn: [
        this.param.date,
        [Validators.required],
      ],
      endedOn: [
        '',
        [Validators.required],
      ],
      reason: [
        '',
        [Validators.required],
      ],
    });
  }
}
