import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Teacher } from '../models/teacher';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TeacherService {
  private apiUrl = `${environment.apiBaseUrl}/Teacher`;

  constructor(private http: HttpClient) {}

  // GET ALL Teachers
  getTeachers(role: string, schoolId?: number): Observable<Teacher[]> {
    let url = `${this.apiUrl}/GetAllTeachers?role=${role}`;
    if (schoolId) {
      url += `&schoolId=${schoolId}`;
    }
    return this.http.get<Teacher[]>(url);
  }

  // GET Teacher BY ID
  getTeacherById(id: number): Observable<Teacher> {
    return this.http.get<Teacher>(`${this.apiUrl}/GetTeacherById?id=${id}`);
  }

  // CREATE / UPDATE Teacher
  manageTeacher(teacher: Teacher): Observable<any> {
    return this.http.post(`${this.apiUrl}/ManageTeacher`, teacher);
  }

  // DELETE Teacher
  deleteTeacher(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteTeacher?id=${id}`);
  }
}
