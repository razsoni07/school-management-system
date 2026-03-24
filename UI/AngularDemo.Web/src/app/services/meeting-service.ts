import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MeetingService {
  private apiUrl = `${environment.apiBaseUrl}/meeting`;

  constructor(private http: HttpClient) { }

  createMeeting(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/CreateMeeting`, data);
  }

  getAllMeetings(role: string, schoolId?: number): Observable<any[]> {
    let url = `${this.apiUrl}/GetAllMeetings?role=${role}`;
    if (schoolId) {
      url += `&schoolId=${schoolId}`;
    }
    return this.http.get<any[]>(url);
  }

  getTeacherMeetings(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetTeacherMeetings`);
  }

  getSchoolMeetings(schoolId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetSchoolMeetings/?schoolId=${schoolId}`);
  }

  joinMeeting(meetingId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/JoinMeeting?meetingId=${meetingId}`, {});
  }
}
