import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authConfig: AuthConfig = {
  issuer: environment.urlIdentity,
  clientId: 'enterprise.web',
  responseType: 'code',
  redirectUri: window.location.origin,
  silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
  scope: 'openid profile email enterprise',
  useSilentRefresh: true,
  silentRefreshTimeout: 5000,
  timeoutFactor: 0.25,
  sessionChecksEnabled: true,
  showDebugInformation: true,
  clearHashAfterLogin: false,
  nonceStateSeparator: 'semicolon',
  dummyClientSecret: 'secret',
};
