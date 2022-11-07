import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

import { SidenavConfig } from './sidenav-config.model';

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  sidenavConfig$: Observable<SidenavConfig> = this.buildSidenavConfig();

  constructor(private breakpointObserver: BreakpointObserver) {}

  private buildSidenavConfig(): Observable<SidenavConfig> {
    return this.breakpointObserver.observe(Breakpoints.Handset).pipe(
      map(breakpointState => {
        return {
          fixedTopGap: 64,
          isHandset: breakpointState.matches,
          mode: breakpointState.matches ? 'over' : 'side',
          role: breakpointState.matches ? 'dialog' : 'navigation',
        } as SidenavConfig;
      }),
    );
  }
}
