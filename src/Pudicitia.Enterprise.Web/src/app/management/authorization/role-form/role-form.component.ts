import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { NamedEntity } from 'shared/models/named-entity.model';

import { AuthorizationService } from '../authorization.service';
import { Role } from '../role.model';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.scss'],
})
export class RoleFormComponent implements OnInit {
  isLoading = true;
  isToUpdate = false;
  role = <Role>{};
  permissions: NamedEntity[] = [];

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private authorizationService: AuthorizationService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    let role$ = this.authorizationService.getNewRole();
    if (Guid.isGuid(id)) {
      this.isToUpdate = true;
      role$ = this.authorizationService.getRole(Guid.parse(id!));
    }
    role$
      .pipe(
        tap(output => {
          this.role = output.role;
          this.permissions = output.permissions;
        }),
        finalize(() => this.isLoading = false),
      )
      .subscribe();
  }

  save(): void {
    let role$ = this.authorizationService.createRole(this.role);
    if (this.isToUpdate) {
      role$ = this.authorizationService.updateRole(this.role);
    }
    role$
      .pipe(tap(() => this.back()))
      .subscribe();
  }

  back(): void {
    this.location.back();
  }
}
