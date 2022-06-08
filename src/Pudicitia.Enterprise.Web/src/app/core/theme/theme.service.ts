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
    this.theme$.next(<Theme>localStorage.getItem(this.key));
    this.theme$
      .pipe(
        tap(theme => localStorage.setItem(this.key, theme)),
      )
      .subscribe();
  }
}
