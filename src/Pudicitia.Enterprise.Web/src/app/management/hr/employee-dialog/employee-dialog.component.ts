import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Guid } from 'src/app/core/guid';
import { SaveMode } from 'src/app/core/save-mode';

import { Department } from '../department';
import { Employee } from '../employee';
import { Gender } from '../gender';
import { Job } from '../job';
import { MaritalStatus } from '../marital-status';

@Component({
  selector: 'app-employee-dialog',
  templateUrl: './employee-dialog.component.html',
  styleUrls: ['./employee-dialog.component.scss'],
})
export class EmployeeDialogComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  employee = new Employee();
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
