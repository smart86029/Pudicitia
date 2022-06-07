import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { OverlayContainer } from '@angular/cdk/overlay';
import { Component } from '@angular/core';
import { map, Observable, tap } from 'rxjs';

import { AuthService } from './auth/auth.service';
import { Theme } from './core/theme/theme.enum';
import { ThemeService } from './core/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'pudicitia-enterprise';
  isDark = false;
  selectedTheme = Theme.Light;
  theme = Theme;
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));

  constructor(
    private overlayContainer: OverlayContainer,
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService,
    private themeService: ThemeService,
  ) {
    this.authService.runInitialLoginSequence();

    this.themeService.theme$
      .pipe(tap(theme => (this.selectedTheme = theme)))
      .subscribe();
  }

  signOut(): void {
    this.authService.signOut();
  }

  changeTheme(): void {
    this.overlayContainer
      .getContainerElement()
      .classList.remove(this.selectedTheme);
    this.isDark = !this.isDark;
    const theme = this.isDark ? this.theme.Dark : this.theme.Light;
    this.overlayContainer.getContainerElement().classList.add(theme);
    this.themeService.theme$.next(theme);
  }
}
