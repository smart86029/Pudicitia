import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Theme } from 'src/app/core/theme/theme.enum';
import { ThemeService } from 'src/app/core/theme/theme.service';

@Component({
  selector: 'app-theme-picker',
  templateUrl: './theme-picker.component.html',
  styleUrls: ['./theme-picker.component.scss'],
})
export class ThemePickerComponent implements OnInit {
  selectedTheme = Theme.Strawberry;
  theme = Theme;

  private subscription = new Subscription();

  constructor(
    private themeService: ThemeService,
    private overlayContainer: OverlayContainer
  ) {}

  ngOnInit(): void {
    this.overlayContainer
      .getContainerElement()
      .classList.add(this.selectedTheme);
    this.subscription.add(
      this.themeService.theme$
        .pipe(tap(theme => (this.selectedTheme = theme)))
        .subscribe()
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
