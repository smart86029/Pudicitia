import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, debounceTime, EMPTY, map, Observable, switchMap, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { NamedEntity } from 'shared/models/named-entity.model';
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
export class EmployeeFormComponent {
  isLoading = true;
  saveMode = SaveMode.Create;
  formGroup: FormGroup = this.initFormGroup();
  formControlUserName = new FormControl('');
  employee$: Observable<Employee> = this.initEmployee();
  users$: Observable<NamedEntity[]> = this.initUsers();
  departments$ = new BehaviorSubject<Department[]>([]);
  jobs: Job[] = [];
  gender = Gender;
  maritalStatus = MaritalStatus;
  canAssignJob = true;
  now = new Date();

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private location: Location,
    private snackBar: MatSnackBar,
    private organizationService: OrganizationService,
  ) { }

  getDepartments = (): Observable<Department[]> => this.departments$;

  displayWithName = (user: NamedEntity): string => user.name;

  onUserSelected = (event: MatAutocompleteSelectedEvent): void =>
    this.formGroup.patchValue({ userId: event.option.value.id });

  onDepartmentChange = (department: Department): void =>
    this.formGroup.patchValue({ departmentId: department.id });

  save(): void {
    const employee = this.formGroup.getRawValue() as Employee;
    const employee$ = this.saveMode === SaveMode.Update
      ? this.organizationService.updateEmployee(employee)
      : this.organizationService.createEmployee(employee);
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

  private initFormGroup(): FormGroup {
    return this.formBuilder.group({
      id: Guid.empty,
      name: ['', [Validators.required]],
      displayName: ['', [Validators.required]],
      birthDate: ['', [Validators.required]],
      gender: Gender.NotKnown,
      maritalStatus: MaritalStatus.NotKnown,
      jobId: Guid.empty,
      startOn: '',
    });
  }

  private initEmployee(): Observable<Employee> {
    return this.route.paramMap
      .pipe(
        tap(() => this.isLoading = true),
        switchMap(paramMap => {
          const id = paramMap.get('id');
          if (Guid.isGuid(id)) {
            this.saveMode = SaveMode.Update;
            this.canAssignJob = false;
            return this.organizationService.getEmployee(Guid.parse(id));
          }
          return this.organizationService.getNewEmployee()
            .pipe(
              tap(output => this.departments$.next(output.departments)),
              tap(output => this.jobs = output.jobs),
              map(output => output.employee),
            );
        }),
        tap(employee => {
          this.formGroup.patchValue(employee);
          this.isLoading = false;
        }),
      );
  }

  private initUsers(): Observable<NamedEntity[]> {
    return this.formControlUserName.valueChanges
      .pipe(
        debounceTime(100),
        switchMap(value => value ? this.organizationService.getUsers(value || '') : EMPTY),
      );
  }
}
