import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';
import { NamedEntity } from 'shared/models/named-entity.model';

import { IdentityService } from '../identity.service';
import { User } from '../user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  isLoading = true;
  isToUpdate = false;
  user = <User>{};
  roles: NamedEntity[] = [];

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private identityService: IdentityService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    let user$ = this.identityService.getNewUser();
    if (Guid.isGuid(id)) {
      this.isToUpdate = true;
      user$ = this.identityService.getUser(Guid.parse(id!));
    }
    user$
      .pipe(
        tap(output => {
          this.user = output.user;
          this.roles = output.roles;
        }),
        finalize(() => this.isLoading = false),
      )
      .subscribe();
  }

  save(): void {
    let user$ = this.identityService.createUser(this.user);
    if (this.isToUpdate) {
      user$ = this.identityService.updateUser(this.user);
    }
    user$
      .pipe(tap(() => this.back()))
      .subscribe();
  }

  back(): void {
    this.location.back();
  }
}
