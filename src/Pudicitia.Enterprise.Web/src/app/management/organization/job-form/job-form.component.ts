import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { SaveMode } from 'shared/models/save-mode.enum';
import { Job } from '../job.model';
import { OrganizationService } from '../organization.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-job-form',
  templateUrl: './job-form.component.html',
  styleUrls: ['./job-form.component.scss'],
})
export class JobFormComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  job = <Job>{};

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
      this.organizationService.getJob(Guid.parse(id))
        .pipe(
          tap(job => {
            this.job = job;
            this.isLoading = false;
          }),
        )
        .subscribe();
    }
  }

  save(): void {
    let job$ = this.organizationService.createJob(this.job);
    if (this.saveMode === SaveMode.Update) {
      job$ = this.organizationService.updateJob(this.job);
    }
    job$
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
