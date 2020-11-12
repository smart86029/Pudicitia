import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

import { Guid } from '../../../shared/models/guid.model';
import { NamedEntity } from '../../../shared/models/named-entity.model';
import { SaveMode } from '../../../shared/models/save-mode.enum';
import { IdentityService } from '../identity.service';
import { RoleOutput } from '../role-output.model';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.scss'],
})
export class RoleDetailComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
  role = <Role>{};
  permissions: NamedEntity[];

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private identityService: IdentityService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    let role$ = this.identityService.getNewRole();
    if (Guid.isGuid(id)) {
      this.saveMode = SaveMode.Update;
      role$ = this.identityService.getRole(Guid.parse(id));
    }
    role$
      .pipe(
        tap(output => {
          this.role = output.role;
          this.permissions = output.permissions;
        }),
        finalize(() => (this.isLoading = false))
      )
      .subscribe();
  }

  save(): void {
    let role$ = this.identityService.createRole(this.role);
    if (this.saveMode === SaveMode.Update) {
      role$ = this.identityService.updateRole(this.role);
    }
    role$.pipe(tap(() => this.back())).subscribe();
  }

  back(): void {
    this.location.back();
  }
}
