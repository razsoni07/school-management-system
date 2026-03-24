import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { StudentService } from '../../../services/student-service';
import { SchoolService } from '../../../services/school-service';
import { CommonService } from '../../../services/common-service';
import { Auth } from '../../../services/auth';
import { Student } from '../../../models/student';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatOptionModule,
    MatSlideToggleModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './student-form.html',
  styleUrl: './student-form.css',
})
export class StudentForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private studentService = inject(StudentService);
  private schoolService = inject(SchoolService);
  private commonService = inject(CommonService);
  private auth = inject(Auth);

  studentId = 0;
  isEdit = false;
  loggedInRole = '';
  loggedInSchoolId?: number;
  schools: any[] = [];
  academicYears: any[] = [];
  classes: { id: number; name: string }[] = [];
  sections: { id: number; sectionName: string }[] = [];

  form = this.fb.group({
    FullName: ['', Validators.required],
    UserName: ['', Validators.required],
    Password: [''],
    RollNumber: ['', Validators.required],
    DateOfBirth: ['', Validators.required],
    Gender: [''],
    ClassMasterId: [null as number | null, Validators.required],
    SectionId: [null as number | null],
    AdmissionDate: ['', Validators.required],
    Email: ['', [Validators.required, Validators.email]],
    Phone: [''],
    Address: [''],
    SchoolId: [null as number | null],
    AcademicYearId: [null as number | null],
    IsActive: [true]
  });

  ngOnInit(): void {
    const user = this.auth.getUserInfo();
    this.loggedInRole = user?.role ?? '';
    this.loggedInSchoolId = user?.schoolId;

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.studentId) {
      this.isEdit = true;
      this.loadStudent();
    }

    this.schoolService.getAllSchools(this.loggedInRole, this.loggedInSchoolId).subscribe(data => {
      this.schools = data;
    });

    this.commonService.getAllAcademicYear().subscribe(data => {
      this.academicYears = data;
    });

    this.commonService.getAllClasses().subscribe(data => {
      this.classes = data;
    });

    this.form.get('ClassMasterId')!.valueChanges.subscribe(classId => {
      this.sections = [];
      this.form.get('SectionId')!.setValue(null, { emitEvent: false });
      if (classId) {
        this.commonService.getSectionsByClassId(classId).subscribe(data => {
          this.sections = data;
        });
      }
    });
  }

  loadStudent() {
    this.studentService.getStudentById(this.studentId).subscribe(student => {
      if (student.classMasterId) {
        this.commonService.getSectionsByClassId(student.classMasterId).subscribe(data => {
          this.sections = data;
          this.form.patchValue({ SectionId: student.sectionId ?? null }, { emitEvent: false });
        });
      }
      this.form.patchValue({
        FullName: student.fullName,
        UserName: student.userName,
        RollNumber: student.rollNumber,
        DateOfBirth: student.dateOfBirth,
        Gender: student.gender,
        ClassMasterId: student.classMasterId ?? null,
        SectionId: student.sectionId ?? null,
        AdmissionDate: student.admissionDate,
        Email: student.email,
        Phone: student.phone,
        Address: student.address,
        SchoolId: student.schoolId ?? null,
        AcademicYearId: student.academicYearId ?? null,
        IsActive: student.isActive
      });
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: Student = {
      id: this.isEdit ? this.studentId : 0,
      fullName: this.form.value.FullName ?? '',
      userName: this.form.value.UserName ?? '',
      password: this.form.value.Password ?? '',
      rollNumber: this.form.value.RollNumber ?? '',
      dateOfBirth: this.form.value.DateOfBirth ?? '',
      gender: this.form.value.Gender ?? '',
      classMasterId: this.form.value.ClassMasterId ?? undefined,
      sectionId: this.form.value.SectionId ?? undefined,
      admissionDate: this.form.value.AdmissionDate ?? '',
      email: this.form.value.Email ?? '',
      phone: this.form.value.Phone ?? '',
      address: this.form.value.Address ?? '',
      schoolId: this.form.value.SchoolId ?? undefined,
      academicYearId: this.form.value.AcademicYearId ?? undefined,
      isActive: this.form.value.IsActive ?? true
    };

    this.studentService.manageStudent(request, this.loggedInRole, this.loggedInSchoolId).subscribe(() => {
      this.router.navigate(['/students']);
    });
  }

  cancel() {
    this.router.navigate(['/students']);
  }
}
