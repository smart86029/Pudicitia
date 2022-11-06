import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { interval, map, Observable, throttle } from 'rxjs';
import { MenuGroup } from 'shared/models/menu-group';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss'],
})
export class ManagementComponent {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));
  menuGroups$: Observable<MenuGroup[]> = this.buildMenuGroups();

  constructor(private breakpointObserver: BreakpointObserver, private authService: AuthService) {}

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
