import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthErrorEvent, OAuthService } from 'angular-oauth2-oidc';
import {
  BehaviorSubject,
  combineLatest,
  Observable,
  ReplaySubject,
} from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isAuthenticated$: Observable<boolean>;
  isDoneLoading$: Observable<boolean>;
  canActivateProtectedRoutes$: Observable<boolean>;

  private isAuthenticatedSubject$ = new BehaviorSubject<boolean>(false);
  private isDoneLoadingSubject$ = new ReplaySubject<boolean>();

  constructor(private oauthService: OAuthService, private router: Router) {
    this.isAuthenticated$ = this.isAuthenticatedSubject$.asObservable();
    this.isDoneLoading$ = this.isDoneLoadingSubject$.asObservable();
    this.canActivateProtectedRoutes$ = combineLatest([
      this.isAuthenticated$,
      this.isDoneLoading$,
    ]).pipe(map(values => values.every(x => x)));

    this.oauthService.events.subscribe(event => {
      if (event instanceof OAuthErrorEvent) {
        console.error('OAuthErrorEvent Object:', event);
      } else {
        console.warn('OAuthEvent Object:', event);
      }
    });

    window.addEventListener('storage', event => {
      if (event.key !== 'access_token' && event.key !== null) {
        return;
      }

      console.warn(
        'Noticed changes to access_token (most likely from another tab), updating isAuthenticated'
      );
      this.isAuthenticatedSubject$.next(
        this.oauthService.hasValidAccessToken()
      );

      if (!this.oauthService.hasValidAccessToken()) {
        this.oauthService.initLoginFlow();
      }
    });

    this.oauthService.events
      .pipe(
        tap(_ =>
          this.isAuthenticatedSubject$.next(
            this.oauthService.hasValidAccessToken()
          )
        )
      )
      .subscribe();

    this.oauthService.events
      .pipe(
        filter(event => ['token_received'].includes(event.type)),
        tap(_ => this.oauthService.loadUserProfile())
      )
      .subscribe();

    this.oauthService.events
      .pipe(
        filter(event =>
          ['session_terminated', 'session_error'].includes(event.type)
        ),
        tap(_ => this.oauthService.initLoginFlow())
      )
      .subscribe();

    this.oauthService.setupAutomaticSilentRefresh();
  }

  runInitialLoginSequence(): Promise<void> {
    if (location.hash) {
      console.log('Encountered hash fragment, plotting as table...');
      console.table(
        location.hash
          .substr(1)
          .split('&')
          .map(kvp => kvp.split('='))
      );
    }

    return this.oauthService
      .loadDiscoveryDocument()
      .then(() => new Promise(resolve => setTimeout(() => resolve(), 1000)))
      .then(() => this.oauthService.tryLogin())
      .then(() => {
        if (this.oauthService.hasValidAccessToken()) {
          return Promise.resolve();
        }

        return this.oauthService
          .silentRefresh()
          .then(() => Promise.resolve())
          .catch(result => {
            const errorResponsesRequiringUserInteraction = [
              'interaction_required',
              'login_required',
              'account_selection_required',
              'consent_required',
            ];

            if (
              result &&
              result.reason &&
              errorResponsesRequiringUserInteraction.indexOf(
                result.reason.error
              ) >= 0
            ) {
              console.warn(
                'User interaction is needed to log in, we will wait for the user to manually log in.'
              );
              return Promise.resolve();
            }

            return Promise.reject(result);
          });
      })
      .then(() => {
        this.isDoneLoadingSubject$.next(true);
        if (!!this.oauthService.state) {
          let stateUrl = this.oauthService.state;
          if (!stateUrl.startsWith('/')) {
            stateUrl = decodeURIComponent(stateUrl);
          }
          console.log(
            `There was state of ${this.oauthService.state}, so we are sending you to: ${stateUrl}`
          );
          this.router.navigateByUrl(stateUrl);
        }
      })
      .catch(() => this.isDoneLoadingSubject$.next(true));
  }

  signIn(targetUrl?: string): void {
    this.oauthService.initLoginFlow(targetUrl || this.router.url);
  }

  signOut(): void {
    this.oauthService.logOut();
  }

  refresh(): void {
    this.oauthService.silentRefresh();
  }

  hasValidToken(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  hasPermission(permission: string): boolean {
    var scope = this.oauthService.getGrantedScopes();
    console.log(scope);
    return true;
  }
}
