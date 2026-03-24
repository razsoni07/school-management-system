import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatOption, MatOptionModule } from '@angular/material/core';
import { TeacherService } from '../../../services/teacher-service';
import { SchoolService } from '../../../services/school-service';
import { CommonService } from '../../../services/common-service';
import { Router, ActivatedRoute } from '@angular/router';
import { Auth } from '../../../services/auth';
import { MatSelectModule } from '@angular/material/select';
import { Department } from '../../../models/department';

@Component({
  selector: 'app-teacher-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatOption,
    MatOptionModule,
    MatSelectModule
  ],
  templateUrl: './teacher-form.html',
  styleUrl: './teacher-form.css',
})
export class TeacherForm implements OnInit {
  form!: FormGroup;
  isEdit = false;
  loggedInUserId = '';
  loggedInRole = '';
  loggedInSchoolId?: number;
  teacherId = 0;
  schools: any[] = [];
  departments: Department[] = [];

  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private schoolService: SchoolService,
    private commonService: CommonService,
    private router: Router,
    private auth: Auth,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      Id: [0],
      SchoolId: [null, Validators.required],
      FullName: ['', Validators.required],
      EmployeeCode: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Phone: [''],
      UserName: ['', Validators.required],
      Password: ['', Validators.required],
      DepartmentId: [null, Validators.required],
      Qualification: ['', Validators.required],
      ExperienceYears: [0, Validators.required],
      JoiningDate: ['', Validators.required],
      Subjects: [''],
      ClassTeacherOf: [''],
      IsActive: [true]
    });
    
    // Logged in user info
    const user = this.auth.getUserInfo();
    this.loggedInUserId = user?.userId.toString() ?? '';
    this.loggedInRole = user?.role;
    this.loggedInSchoolId = user?.schoolId;

    // Edit mode
    this.teacherId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.teacherId) {
      this.isEdit = true;
      this.loadTeacher();
    }

    this.schoolService.getAllSchools(this.loggedInRole, this.loggedInSchoolId).subscribe(data => {
      this.schools = data;
    });

    this.commonService.getAllDepartments().subscribe(data => {
      this.departments = data;
    });
  }

  loadTeacher() {
    this.teacherService.getTeacherById(this.teacherId).subscribe(teacher => {
      debugger
      this.form.patchValue({
        Id: teacher.id,
        SchoolId: teacher.schoolId,
        FullName: teacher.fullName,
        EmployeeCode: teacher.employeeCode,
        Email: teacher.email,
        Phone: teacher.phone,
        UserName: teacher.userName,
        DepartmentId: teacher.departmentId,
        Qualification: teacher.qualification,
        ExperienceYears: teacher.experienceYears,
        JoiningDate: teacher.joiningDate,
        Subjects: teacher.subjects,
        ClassTeacherOf: teacher.classTeacherOf,
        IsActive: teacher.isActive
      });
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.teacherService.manageTeacher(this.form.value).subscribe(() => {
      this.router.navigate(['/teachers']);
    });
  }

  cancel() {
    this.router.navigate(['/teachers']);
  }
}
