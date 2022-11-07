import { MatDrawerMode } from '@angular/material/sidenav';

export interface SidenavConfig {
  fixedTopGap: number;
  isHandset: boolean;
  mode: MatDrawerMode;
  role: string;
}
