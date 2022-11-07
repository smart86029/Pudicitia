import { Component } from '@angular/core';
import { interval, map, Observable, throttle } from 'rxjs';
import { LayoutService } from 'shared/material/layout/layout.service';
import { SidenavConfig } from 'shared/material/layout/sidenav-config.model';
import { MenuGroup } from 'shared/models/menu-group';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss'],
})
export class ManagementComponent {
  sidenavConfig$: Observable<SidenavConfig> = this.layoutService.sidenavConfig$;
  menuGroups$: Observable<MenuGroup[]> = this.buildMenuGroups();

  constructor(private authService: AuthService, private layoutService: LayoutService) {}

  private buildMenuGroups(): Observable<MenuGroup[]> {
    return this.authService.isAuthenticated$.pipe(
      throttle(() => interval(1000 * 30)),
      map(() => {
        const menuGroups: MenuGroup[] = [
          {
            name: 'Identity',
            menus: [{ name: 'Authorization', url: 'authorization', icon: 'group' }],
          },
        ];
        if (this.authService.hasPermission('HumanResources')) {
          menuGroups.push({
            name: 'Human Resources',
            menus: [
              { name: 'Organization', url: 'organization', icon: 'account_tree' },
              { name: 'Attendance', url: 'attendance', icon: 'edit_calendar' },
            ],
          });
        }
        return menuGroups;
      }),
    );
  }
}
