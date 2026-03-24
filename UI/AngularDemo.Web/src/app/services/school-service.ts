import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { School } from '../models/school';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SchoolService {
  private apiUrl = `${environment.apiBaseUrl}/School`;

  constructor(private http: HttpClient) {}

  // ==============================
  // GET ALL Schools
  // ==============================
  getAllSchools(role: string, schoolId?: number): Observable<School[]> {
    let url = `${this.apiUrl}/GetAllSchools?role=${role}`;
    if (schoolId) {
      url += `&schoolId=${schoolId}`;
    }
    return this.http.get<School[]>(url);
  }

  // ==============================
  // GET School BY ID
  // ==============================
  getSchoolById(id: number): Observable<School> {
    return this.http.get<School>(`${this.apiUrl}/GetSchoolById?id=${id}`);
  }

  // ==============================
  // CREATE / UPDATE School
  // ==============================
  manageSchool(school: School): Observable<any> {
    return this.http.post(`${this.apiUrl}/ManageSchool`, school);
  }

  // ==============================
  // DELETE School
  // ==============================
  deleteSchool(id: number): Observable<any> {
    debugger
    return this.http.delete(`${this.apiUrl}/DeleteSchool?id=${id}`);
  }

  toggleSchoolStatus(id: number): Observable<any> {
    debugger
    return this.http.put(`${this.apiUrl}/ToggleSchoolStatus?id=${id}`, {});
  }
}
