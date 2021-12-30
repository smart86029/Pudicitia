import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Theme } from 'core/theme/theme.enum';
import { ThemeService } from 'core/theme/theme.service';
import { Subscription, tap } from 'rxjs';

@Component({
  selector: 'app-theme-picker',
  templateUrl: './theme-picker.component.html',
  styleUrls: ['./theme-picker.component.scss'],
})
export class ThemePickerComponent implements OnInit, OnDestroy {
  selectedTheme = Theme.Strawberry;
  theme = Theme;

  private subscription = new Subscription();

  constructor(
    private overlayContainer: OverlayContainer,
    private themeService: ThemeService,
  ) { }

  ngOnInit(): void {
    this.overlayContainer
      .getContainerElement()
      .classList.add(this.selectedTheme);
    this.subscription.add(
      this.themeService.theme$
        .pipe(tap(theme => (this.selectedTheme = theme)))
        .subscribe(),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  changeTheme(theme: Theme): void {
    this.overlayContainer
      .getContainerElement()
      .classList.remove(this.selectedTheme);
    this.overlayContainer.getContainerElement().classList.add(theme);
    this.themeService.theme$.next(theme);
  }
}
