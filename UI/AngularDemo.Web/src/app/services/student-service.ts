import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student } from '../models/student';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private apiUrl = `${environment.apiBaseUrl}/Student`;

  constructor(private http: HttpClient) {}

  // ==============================
  // GET ALL Students
  // ==============================
  getAllStudents(role: string, schoolId?: number): Observable<Student[]> {
    let url = `${this.apiUrl}/GetAllStudents?role=${role}`;
    if (schoolId) {
      url += `&schoolId=${schoolId}`;
    }
    return this.http.get<Student[]>(url);
  }

  // ==============================
  // GET Student BY ID
  // ==============================
  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/GetStudentById?id=${id}`);
  }

  // ==============================
  // CREATE / UPDATE Student
  // ==============================
  manageStudent(model: Student, role: string, userSchoolId?: number): Observable<any> {
    let url = `${this.apiUrl}/ManageStudent?role=${role}`;
    if (userSchoolId) {
      url += `&userSchoolId=${userSchoolId}`;
    }
    return this.http.post(url, model);
  }

  // ==============================
  // DELETE Student
  // ==============================
  deleteStudent(id: number, role: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteStudent?id=${id}&role=${role}`);
  }

  // ==============================
  // TOGGLE Student Status
  // ==============================
  toggleStudentStatus(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/ToggleStudentStatus?id=${id}`, {});
  }

  // ==============================
  // GET Student Count
  // ==============================
  getStudentCount(role: string, schoolId?: number): Observable<{ count: number }> {
    let url = `${this.apiUrl}/GetStudentCount?role=${role}`;
    if (schoolId) url += `&schoolId=${schoolId}`;
    return this.http.get<{ count: number }>(url);
  }
}
