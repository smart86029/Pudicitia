import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { SaveMode } from '../../../shared/models/save-mode.enum';
import { Employee } from '../employee.model';
import { Gender } from '../gender.enum';
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
  departmentName: string;
  jobs: Job[] = [];
  gender = Gender;
  maritalStatus = MaritalStatus;
  canAssignJob = true;
  now = new Date();

  constructor(@Inject(MAT_DIALOG_DATA) private data: any) {
    data.jobs.forEach(job => this.jobs.push(job));
    this.employee.departmentId = data.department.id;
    this.departmentName = data.department.name;
  }

  ngOnInit(): void {
    this.isLoading = false;
  }
}
