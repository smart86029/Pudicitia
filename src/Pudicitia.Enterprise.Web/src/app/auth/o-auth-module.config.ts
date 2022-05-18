import { OAuthModuleConfig } from 'angular-oauth2-oidc';

export const oAuthModuleConfig: OAuthModuleConfig = {
  resourceServer: {
    allowedUrls: ['api'],
    sendAccessToken: true,
  },
};
