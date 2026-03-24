import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Department } from '../models/department';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getAllDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(`${this.baseUrl}/Department/GetAllDepartments`);
  }

  getAllClasses(): Observable<{ id: number; name: string }[]> {
    return this.http.get<{ id: number; name: string }[]>(`${this.baseUrl}/Class/GetAllClasses`);
  }

  getAllAcademicYear(): Observable<{ id: number; name: string }[]> {
    return this.http.get<{ id: number; name: string }[]>(`${this.baseUrl}/AcademicYear/GetAllAcademicYear`);
  }

  getSectionsByClassId(classMasterId: number): Observable<{ id: number; sectionName: string }[]> {
    return this.http.get<{ id: number; sectionName: string }[]>(
      `${this.baseUrl}/Section/GetSectionsByClass?classId=${classMasterId}`
    );
  }

  getAvailableSectionsByTeacher(teacherId: number, schoolId: number): Observable<{ id: number; sectionName: string }[]> {
    return this.http.get<{ id: number; sectionName: string }[]>(
      `${this.baseUrl}/Section/GetAvailableSectionsByTeacher?teacherId=${teacherId}&schoolId=${schoolId}`
    );
  }
}
