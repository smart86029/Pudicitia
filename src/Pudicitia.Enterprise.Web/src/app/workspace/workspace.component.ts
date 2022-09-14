import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { interval, map, Observable, tap, throttle } from 'rxjs';
import { MenuGroup } from 'shared/models/menu-group';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.scss'],
})
export class WorkspaceComponent implements OnInit {
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
        name: 'Personal',
        menus: [
          { name: 'Schedule', url: 'schedule', icon: 'event' },
        ],
      },
    ];
    return menuGroups;
  }
}
