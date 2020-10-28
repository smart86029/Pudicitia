import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { Theme } from './theme.enum';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  theme$: BehaviorSubject<Theme> = new BehaviorSubject<Theme>(Theme.Strawberry);

  constructor() {}
}
