import { Department } from './department.model';
import { Employee } from './employee.model';
import { Job } from './job.model';

export interface EmployeeOutput {
  employee: Employee;
  departments: Department[];
  jobs: Job[];
}
