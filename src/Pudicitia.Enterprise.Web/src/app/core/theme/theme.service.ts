import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';

import { Theme } from './theme.enum';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  theme$: BehaviorSubject<Theme> = new BehaviorSubject<Theme>(Theme.Light);

  private key = 'theme';

  constructor() {
    const theme = <Theme>localStorage.getItem(this.key);
    if (theme) {
      this.theme$.next(theme);
    }
    this.theme$
      .pipe(
        tap(theme => localStorage.setItem(this.key, theme)),
      )
      .subscribe();
  }
}
