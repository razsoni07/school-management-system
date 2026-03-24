import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private http: HttpClient) {}
  private apiUrl = `${environment.apiBaseUrl}/Dashboard`;

  getAdminStats() {
    debugger
    return this.http.get<any>(`${this.apiUrl}/GetAdminStats`);
  }

  getPrincipalStats() {
    debugger
    return this.http.get<any>(`${this.apiUrl}/GetPrincipalStats`);
  }
}
