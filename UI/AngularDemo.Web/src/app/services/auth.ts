import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EMPTY, from, switchMap, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { msalInstance } from '../core/msal.config';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = `${environment.apiBaseUrl}/Auth`;

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/login`, {
      userName: username,
      password: password
    });
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
  }

  loginWithMicrosoft() {
    // initialize() is idempotent — safe to call every time, resolves instantly after first call.
    // redirectUri points to the lightweight redirect page, not the full Angular app,
    // which prevents redirect_bridge_timeout.
    const popup$ = from(
      msalInstance.initialize().then(() =>
        msalInstance.loginPopup({
          scopes: ['openid', 'profile', 'User.Read'],
          redirectUri: `${window.location.origin}/auth-redirect.html`
        })
      )
    );

    return popup$.pipe(
      // If user closes the popup or there's a leftover interaction state,
      // complete silently — no error, button re-enables via complete()
      catchError(err => {
        const silent = ['user_cancelled', 'popup_window_error', 'interaction_in_progress'];
        if (silent.includes(err?.errorCode)) return EMPTY;
        return throwError(() => err);
      }),
      switchMap(result =>
        this.http.post<any>(`${this.apiUrl}/AzureLogin`, {
          idToken: result.idToken
        })
      ),
      tap(response => this.saveToken(response.token))
    );
  }

  getUserInfo() {
    const token = localStorage.getItem('token');
    if(!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));

    return {
      userId: Number(payload.sub),
      username: payload.unique_name || payload.name,
      fullName: payload.fullName || payload.given_name || payload.unique_name,
      role: payload.role || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
      schoolId: payload.schoolId ? Number(payload.schoolId) : undefined,
      schoolName: payload.schoolName || ''
    };
  }
}
