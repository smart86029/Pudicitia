import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { filter, Observable, switchMap, tap } from 'rxjs';

import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authService.isDoneLoading$.pipe(
      filter(isDone => isDone),
      switchMap(() => this.authService.isAuthenticated$),
      tap(isAuthenticated => isAuthenticated || this.authService.signIn(state.url)),
    );
  }
}
