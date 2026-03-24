import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {

  private apiUrl = `${environment.apiBaseUrl}/User`;

  constructor(private http: HttpClient) {}

  // ==============================
  // GET ALL USERS
  // ==============================
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/GetAllUsers`);
  }

  // ==============================
  // GET USER BY ID
  // ==============================
  getUserById(id: number): Observable<User> {
    debugger
    return this.http.get<User>(`${this.apiUrl}/GetUserById?id=${id}`);
  }

  // ==============================
  // CREATE / UPDATE USER
  // ==============================
  manageUser(user: User): Observable<any> {
    debugger
    return this.http.post(`${this.apiUrl}/ManageUser`, user);
  }

  // ==============================
  // DELETE USER
  // ==============================
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteUser?id=${id}`);
  }
}
