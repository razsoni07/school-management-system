import { Component, OnInit, ViewChild, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MeetingService } from '../../services/meeting-service';
import { MeetingType, MeetingTypeOptions, ParticipantType, ParticipantTypeOptions } from '../../helpers/enums';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialog } from '../../shared/confirm-dialog/confirm-dialog';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Meeting } from '../../models/meeting';
import { Auth } from '../../services/auth';
import { SignalrService } from '../../services/signalr-service';

@Component({
  selector: 'app-meeting',
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatSlideToggleModule
  ],
  templateUrl: './meetings.html',
  styleUrl: './meetings.css',
})
export class Meetings implements OnInit {
  meetings: any[] = [];
  loggedInUserId = '';
  loggedInRole = '';
  loggedInSchoolId?: number;

  displayedColumns: string[] = [
    'title',
    'description',
    'schoolName',
    'roomName',
    'meetingType',
    'meetingDate',
    'startTime',
    'endTime',
    'participants',
    'actions'
  ];

  dataSource = new MatTableDataSource<Meeting>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private meetingService: MeetingService,
    private router: Router,
    private auth: Auth,
    private signalRService: SignalrService
  ) {}

  ngOnInit(): void {
     // Logged in user info
    const user = this.auth.getUserInfo();
    this.loggedInUserId = user?.userId.toString() ?? '';
    this.loggedInRole = user?.role ?? '';
    this.loggedInSchoolId = user?.schoolId;

    this.signalRService.startConnection();
    this.signalRService.addMeetingListener();

    this.loadMeetings();
  }

  loadMeetings() {
    if (this.loggedInRole === 'SuperAdmin') {
      this.meetingService.getAllMeetings(this.loggedInRole, this.loggedInSchoolId).subscribe({
        next: res => this.dataSource.data = res,
        error: err => console.error(err)
      });
    } else if (this.loggedInSchoolId) {
      this.meetingService.getSchoolMeetings(this.loggedInSchoolId).subscribe({
        next: res => this.dataSource.data = res,
        error: err => console.error(err)
      });
    }
  }

  join(meetingId: number, roomName: string) {
    this.router.navigate(['/meetings/join', meetingId, roomName]);
  }

  isLive(meeting: any): boolean {
    const now = new Date();
    const meetingDate = new Date(meeting.meetingDate);

    if (meetingDate.toDateString() !== now.toDateString())
      return false;

    const currentTime = now.getHours() * 60 + now.getMinutes();

    const start = this.timeToMinutes(meeting.startTime);
    const end = this.timeToMinutes(meeting.endTime);

    return currentTime >= start && currentTime <= end;
  }

  timeToMinutes(time: string): number {
    const parts = time.split(':');
    return (+parts[0]) * 60 + (+parts[1]);
  }

  addMeeting() {
    this.router.navigate(['/meetings/schedule-meeting']);
  }

  getMeetingTypeLabel(value: number): string {
    return MeetingTypeOptions.find(o => o.value === value)?.label ?? value?.toString();
  }

  getParticipantTypeLabel(value: number): string {
    return ParticipantTypeOptions.find(o => o.value === value)?.label ?? value?.toString();
  }

  getParticipantsLabel(participants: { participantType: number }[]): string {
    if (!participants?.length) return '—';
    return participants.map(p => this.getParticipantTypeLabel(p.participantType)).join(', ');
  }
}
