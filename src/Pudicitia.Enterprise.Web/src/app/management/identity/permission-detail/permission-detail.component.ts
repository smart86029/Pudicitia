import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { finalize, tap } from 'rxjs/operators';

import { Guid } from '../../../shared/models/guid.model';
import { SaveMode } from '../../../shared/models/save-mode.enum';
import { IdentityService } from '../identity.service';
import { Permission } from '../permission.model';

@Component({
  selector: 'app-permission-detail',
  templateUrl: './permission-detail.component.html',
  styleUrls: ['./permission-detail.component.scss'],
})
export class PermissionDetailComponent implements OnInit {
  isLoading = true;
  saveMode = SaveMode.Create;
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
      this.saveMode = SaveMode.Update;
      permission$ = this.identityService.getPermission(Guid.parse(id));
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
    if (this.saveMode === SaveMode.Update) {
      permission$ = this.identityService.updatePermission(this.permission);
    }
    permission$
      .pipe(
        tap(() => {
          this.snackBar.open(`${this.saveMode}d`);
          this.back();
        }),
      )
      .subscribe();
  }

  back(): void {
    this.location.back();
  }
}
