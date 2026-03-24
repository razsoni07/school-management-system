import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MeetingService } from '../../../services/meeting-service';
import { SchoolService } from '../../../services/school-service';
import { CommonService } from '../../../services/common-service';
import { Router } from '@angular/router';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';
import { ParticipantTypeOptions, MeetingTypeOptions, MeetingType, ParticipantType } from '../../../helpers/enums';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatOption } from '@angular/material/select';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTimepicker, MatTimepickerModule } from '@angular/material/timepicker';

@Component({
  selector: 'app-schedule-meeting',
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
    MatSlideToggleModule,
    MatOption,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTimepicker,
    MatTimepickerModule
  ],
  templateUrl: './schedule-meeting.html',
  styleUrl: './schedule-meeting.css',
})
export class ScheduleMeeting implements OnInit {
  form!: FormGroup;
  loggedInUserId = '';
  loggedInRole = '';
  loggedInSchoolId?: number;
  schools: any[] = [];
  sections: any[] = [];
  participantTypeOptions = ParticipantTypeOptions;
  meetingTypeOptions = MeetingTypeOptions;

  constructor(
    private fb: FormBuilder,
    private meetingService: MeetingService,
    private schoolService: SchoolService,
    private commonService: CommonService,
    private router: Router,
    private auth: Auth
  ) {}

  ngOnInit(): void {
    // Logged in user info
    const user = this.auth.getUserInfo();
    this.loggedInUserId = user?.userId.toString() ?? '';
    this.loggedInRole = user?.role;
    this.loggedInSchoolId = user?.schoolId;

    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      schoolId: ['', Validators.required],
      meetingDate: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      roomName: ['', Validators.required],
      meetingType: ['', Validators.required],
      participantType: ['', Validators.required],
      sectionId: ['']
    });

    // Auto set current IST time
    const now = new Date();
    now.setMinutes(Math.ceil(now.getMinutes() / 5) * 5);
    now.setSeconds(0);

    const end = new Date(now);
    end.setHours(now.getHours() + 1); // default 1 hour meeting

    this.form.patchValue({
      meetingDate: now,
      startTime: new Date(now),
      endTime: new Date(end)
    });

    // If Teacher, default meeting type to Section Meeting, disable it, and load sections
    if (this.loggedInRole === 'Teacher') {
      this.form.patchValue({ meetingType: MeetingType.SectionMeeting });
      this.form.patchValue({ participantType: ParticipantType.Section });
      
      this.form.get('sectionId')?.setValidators(Validators.required);
      this.form.get('sectionId')?.updateValueAndValidity();

      const teacherId = Number(this.loggedInUserId);
      const schoolId = this.loggedInSchoolId ?? 0;
      this.commonService.getAvailableSectionsByTeacher(teacherId, schoolId).subscribe(res => {
        this.sections = res;
      });
    }

    // Load schools only for SuperAdmin
    if (this.loggedInRole === 'SuperAdmin') {
      this.schoolService.getAllSchools(this.loggedInRole, this.loggedInSchoolId).subscribe(res => {
        this.schools = res;
      });
    } else {
      // Auto assign school for SchoolAdmin/Teacher
      const schoolId = 1; // this.authService.getSchoolId();
      this.form.patchValue({ schoolId });
    }
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    const payload: any = {
      title: value.title,
      description: value.description,
      schoolId: Number(value.schoolId),
      meetingDate: this.formatDate(value.meetingDate),
      startTime: this.formatTime(value.startTime),
      endTime: this.formatTime(value.endTime),
      roomName: value.roomName,
      meetingType: Number(value.meetingType),
      participants: [{ participantType: Number(value.participantType) }]
    };

    if (value.sectionId) {
      payload.sectionId = Number(value.sectionId);
    }

    this.meetingService.createMeeting(payload).subscribe({
      next: () => {
        this.router.navigate(['/meetings']);
      },
      error: err => console.error(err)
    });
  }

  cancel() {
    this.router.navigate(['/meetings']);
  }

  getCurrentTimeDate(): Date {
    const now = new Date();
    now.setSeconds(0);
    now.setMilliseconds(0);
    return now;
  }

  getNextHourDate(): Date {
    const now = new Date();
    now.setHours(now.getHours() + 1);
    now.setSeconds(0);
    now.setMilliseconds(0);
    return now;
  }

  formatDate(date: Date): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    
    return `${year}-${month}-${day}T00:00:00`;
  }

  formatTime(date: Date): string {
    const h = date.getHours().toString().padStart(2, '0');
    const m = date.getMinutes().toString().padStart(2, '0');
    return `${h}:${m}:00`;
  }
}
