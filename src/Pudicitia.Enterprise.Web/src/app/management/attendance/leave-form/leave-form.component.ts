import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';

import { ApprovalStatus } from '../approval-status.enum';
import { AttendanceService } from '../attendance.service';
import { LeaveType } from '../leave-type.enum';
import { Leave } from '../leave.model';

@Component({
  selector: 'app-leave-form',
  templateUrl: './leave-form.component.html',
  styleUrls: ['./leave-form.component.scss'],
})
export class LeaveFormComponent implements OnInit {
  isLoading = true;
  leave = <Leave>{};
  leaveType = LeaveType;
  approvalStatus = ApprovalStatus;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private snackBar: MatSnackBar,
    private attendanceService: AttendanceService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.attendanceService.getLeave(Guid.parse(id))
      .pipe(
        tap(leave => this.leave = leave),
        finalize(() => this.isLoading = false),
      )
      .subscribe();
  }
}
