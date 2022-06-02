import { Department } from './department.model';
import { Job } from './job.model';

export interface OrganizationOutput {
  departments: Department[];
  jobs: Job[];
}
