import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs';
import { Guid } from 'shared/models/guid.model';

import { IdentityService } from '../identity.service';
import { Permission } from '../permission.model';

@Component({
  selector: 'app-permission-form',
  templateUrl: './permission-form.component.html',
  styleUrls: ['./permission-form.component.scss'],
})
export class PermissionFormComponent implements OnInit {
  isLoading = true;
  isToUpdate = false;
  permission = <Permission>{};

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private snackBar: MatSnackBar,
    private identityService: IdentityService,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    let permission$ = this.identityService.getNewPermission();
    if (Guid.isGuid(id)) {
      this.isToUpdate = true;
      permission$ = this.identityService.getPermission(Guid.parse(id!));
    }
    permission$
      .pipe(
        tap(permission => this.permission = permission),
        finalize(() => this.isLoading = false),
      )
      .subscribe();
  }

  save(): void {
    let permission$ = this.identityService.createPermission(this.permission);
    if (this.isToUpdate) {
      permission$ = this.identityService.updatePermission(this.permission);
    }
    permission$
      .pipe(
        tap(() => {
          this.snackBar.open(this.isToUpdate ? 'Updated' : 'Created');
          this.back();
        }),
      )
      .subscribe();
  }

  back(): void {
    this.location.back();
  }
}
