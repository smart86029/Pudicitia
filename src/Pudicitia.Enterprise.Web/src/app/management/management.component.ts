import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { interval, map, Observable, tap, throttle } from 'rxjs';

import { AuthService } from '../auth/auth.service';
import { MenuGroup } from './menu-group';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss'],
})
export class ManagementComponent implements OnInit {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));
  menuGroups: MenuGroup[] = [];

  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
  ) { }

  ngOnInit(): void {
    this.authService.isAuthenticated$
      .pipe(
        throttle(() => interval(1000 * 30)),
        tap(() => this.menuGroups = this.getMenuGroups()),
      )
      .subscribe();
  }

  private getMenuGroups(): MenuGroup[] {
    const menuGroups: MenuGroup[] = [
      {
        name: 'Identity',
        menus: [
          { name: 'Authorization', url: 'authorization', icon: 'group' },
        ],
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
  }
}
