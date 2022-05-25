import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';
import { Employee } from '../employee.model';
import { Gender } from '../gender.enum';
import { HRService } from '../hr.service';
import { Job } from '../job.model';
import { MaritalStatus } from '../marital-status.enum';

@Component({
  selector: 'app-employee-dialog',
  templateUrl: './employee-dialog.component.html',
  styleUrls: ['./employee-dialog.component.scss'],
})
export class EmployeeDialogComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  employee = <Employee>{
    birthDate: new Date('1990-01-01'),
    gender: Gender.NotKnown,
    maritalStatus: MaritalStatus.NotKnown,
  };
  departmentName = '';
  jobs: Job[] = [];
  gender = Gender;
  maritalStatus = MaritalStatus;
  canAssignJob = true;
  now = new Date();

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: { employeeId: Guid, jobs: Job[], department: Department },
    private hrService: HRService,
  ) {
    if (data.employeeId) {
      this.saveMode = SaveMode.Update;
      this.canAssignJob = false;
      this.hrService.getEmployee(this.data.employeeId)
        .pipe(
          tap(employee => this.employee = employee),
        )
        .subscribe();
    }
    else {
      data.jobs.forEach((job: Job) => this.jobs.push(job));
      this.employee.departmentId = data.department.id;
    }

    this.departmentName = data.department.name;
  }

  ngOnInit(): void {
    this.isLoading = false;
  }
}
