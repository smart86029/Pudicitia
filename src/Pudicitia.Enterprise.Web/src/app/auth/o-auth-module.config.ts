import { OAuthModuleConfig } from 'angular-oauth2-oidc';

export const oAuthModuleConfig: OAuthModuleConfig = {
  resourceServer: {
    allowedUrls: ['https://demo.identityserver.io/api'],
    sendAccessToken: true,
  },
};
