import { Component, OnInit } from '@angular/core';
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
  departments: Department[];
  jobs: Job[];
  gender = Gender;
  maritalStatus = MaritalStatus;
  canAssignJob = true;
  now = new Date();

  constructor() {}

  ngOnInit(): void {
    this.isLoading = false;
  }
}
