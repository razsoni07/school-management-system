import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contacts } from '../../models/contact';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})

export class ContactsService {
  private apiUrl = `${environment.apiBaseUrl}/Contacts`;

   constructor(private http: HttpClient) {}

  getContacts(): Observable<Contacts[]> {
    return this.http.get<Contacts[]>(this.apiUrl + '/GetAllContacts');
  }

  getContactById(id: string) {
    return this.http.get<any>(`${this.apiUrl}/GetContactById/${id}`)
  } 

  addContact(data: any) {
    return this.http.post(this.apiUrl, data);
  }

  updateContact(id: string, data: any) {
    return this.http.put(`${this.apiUrl}/UpdateContact/${id}`, data);
  }

  deleteContact(id: string) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}