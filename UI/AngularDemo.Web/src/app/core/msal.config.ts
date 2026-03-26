import { PublicClientApplication, BrowserCacheLocation } from '@azure/msal-browser';
import { environment } from '../../environments/environment';

export const msalInstance = new PublicClientApplication({
  auth: {
    clientId: environment.azureClientId,
    // "common" allows both org accounts and personal Microsoft accounts (Outlook, Hotmail)
    authority: `https://login.microsoftonline.com/common`,
    redirectUri: window.location.origin
  },
  cache: {
    cacheLocation: BrowserCacheLocation.LocalStorage
  }
});
