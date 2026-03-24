import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MeetingService } from '../../../services/meeting-service';

@Component({
  selector: 'app-join-meeting',
  imports: [],
  templateUrl: './join-meeting.html',
  styleUrl: './join-meeting.css',
})
export class JoinMeeting implements OnInit {
  meetingUrl!: SafeResourceUrl;

  constructor(
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private meetingService: MeetingService
  ) {}

  ngOnInit(): void {
    const meetingId = Number(this.route.snapshot.paramMap.get('meetingId'));
    const roomName = this.route.snapshot.paramMap.get('roomName');
    const url = `https://meet.jit.si/${roomName}`;
    this.meetingUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);

    if (meetingId) {
      this.meetingService.joinMeeting(meetingId).subscribe({
        error: err => console.error('Failed to record meeting attendance', err)
      });
    }
  }
}
