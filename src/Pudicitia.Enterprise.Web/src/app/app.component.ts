import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, OnInit } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { Menu } from 'shared/models/menu.model';

import { AuthService } from './auth/auth.service';
import { Theme } from './core/theme/theme.enum';
import { ThemeService } from './core/theme/theme.service';
import { ThemeConfig } from './theme-config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'pudicitia-enterprise';
  theme = Theme;
  selectedTheme = Theme.Light;
  themeConfigs = new Map<Theme, ThemeConfig>([
    [Theme.Light, { toolbarColor: 'primary', navColor: 'accent', navBackgroundColor: 'primary', icon: 'dark_mode' }],
    [Theme.Dark, { toolbarColor: undefined, navColor: 'primary', navBackgroundColor: undefined, icon: 'light_mode' }],
  ]);
  themeConfig = this.themeConfigs.get(this.selectedTheme)!;
  menus: Menu[] = [
    { name: 'workspace', url: 'workspace' },
    { name: 'management', url: 'management' },
  ];
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));

  constructor(
    private overlayContainer: OverlayContainer,
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private themeService: ThemeService,
  ) { }

  ngOnInit(): void {
    this.authService.runInitialLoginSequence();
    this.themeService.theme$
      .pipe(
        tap(theme => {
          const classList = this.overlayContainer.getContainerElement().classList;
          classList.remove(this.selectedTheme);
          classList.add(theme);
          this.selectedTheme = theme;
          this.themeConfig = this.themeConfigs.get(theme)!;
        }),
      )
      .subscribe();
  }

  signOut(): void {
    this.authService.signOut();
  }

  changeTheme(): void {
    const theme = this.selectedTheme == this.theme.Dark ? this.theme.Light : this.theme.Dark;
    this.themeService.theme$.next(theme);
  }
}
