import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

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
