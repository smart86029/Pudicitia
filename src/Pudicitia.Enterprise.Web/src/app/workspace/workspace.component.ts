import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { interval, map, Observable, throttle } from 'rxjs';
import { MenuGroup } from 'shared/models/menu-group';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.scss'],
})
export class WorkspaceComponent {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));
  menuGroups$: Observable<MenuGroup[]> = this.buildMenuGroups();

  constructor(private breakpointObserver: BreakpointObserver, private authService: AuthService) {}

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
