import { Component } from '@angular/core';
import { interval, map, Observable, throttle } from 'rxjs';
import { LayoutService } from 'shared/material/layout/layout.service';
import { SidenavConfig } from 'shared/material/layout/sidenav-config.model';
import { MenuGroup } from 'shared/models/menu-group';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.scss'],
})
export class WorkspaceComponent {
  sidenavConfig$: Observable<SidenavConfig> = this.layoutService.sidenavConfig$;
  menuGroups$: Observable<MenuGroup[]> = this.buildMenuGroups();

  constructor(private authService: AuthService, private layoutService: LayoutService) {}

  private buildMenuGroups(): Observable<MenuGroup[]> {
    return this.authService.isAuthenticated$.pipe(
      throttle(() => interval(1000 * 30)),
      map(() => [
        {
          name: 'Personal',
          menus: [
            { name: 'Schedule', url: 'schedule', icon: 'event' },
          ],
        },
      ]),
    );
  }
}
