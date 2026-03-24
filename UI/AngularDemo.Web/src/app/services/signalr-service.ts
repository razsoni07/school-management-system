import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection!: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/meetingHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('Error connecting to SignalR', err));
  }

  addMeetingListener() {
    this.hubConnection.on('ReceiveMeetingNotification', (meeting) => {
      console.log('New Meeting Notification:', meeting);
    });
  }
}
