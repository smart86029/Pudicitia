import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { Department } from '../department.model';
import { Employee } from '../employee.model';
import { Gender } from '../gender.enum';
import { Job } from '../job.model';
import { MaritalStatus } from '../marital-status.enum';
import { OrganizationService } from '../organization.service';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss'],
})
export class EmployeeFormComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  employee = <Employee>{
    birthDate: new Date('1990-01-01'),
    gender: Gender.NotKnown,
    maritalStatus: MaritalStatus.NotKnown,
  };
  departmentName = '';
  departments$ = new BehaviorSubject<Department[]>([]);
  jobs: Job[] = [];
  gender = Gender;
  maritalStatus = MaritalStatus;
  canAssignJob = true;
  now = new Date();

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (Guid.isGuid(id)) {
      this.saveMode = SaveMode.Update;
      this.canAssignJob = false;
      this.organizationService.getEmployee(Guid.parse(id))
        .pipe(
          tap(employee => {
            this.employee = employee;
          }),
          finalize(() => this.isLoading = false),
        )
        .subscribe();
    } else {
      const departmentId = Guid.parse(this.route.snapshot.paramMap.get('departmentId'));
      this.employee.departmentId = departmentId;
      this.organizationService.getNewEmployee()
        .pipe(
          tap(output => {
            this.departments$.next(output.departments);
            output.jobs.forEach((job: Job) => this.jobs.push(job));
          }),
          finalize(() => this.isLoading = false),
        )
        .subscribe();
    }
  }

  getDepartments = () => this.departments$;

  save(): void {
    let employee$ = this.organizationService.createEmployee(this.employee);
    if (this.saveMode === SaveMode.Update) {
      employee$ = this.organizationService.updateEmployee(this.employee);
    }
    employee$
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
}
