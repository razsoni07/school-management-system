import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from '../../../services/user-service';
import { SchoolService } from '../../../services/school-service';
import { Auth } from '../../../services/auth';
import { User } from '../../../models/user';
import { UserRole } from '../../../helpers/enums';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule
  ],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css',
})
export class UserForm implements OnInit {

  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private userService = inject(UserService);
  private schoolService = inject(SchoolService);
  private auth = inject(Auth);

  userId = 0;
  isEdit = false;
  loggedInUserId = '';
  loggedInRole = '';
  loggedInSchoolId?: number;

  roles = Object.values(UserRole);

  // For Multi-Tenant (SuperAdmin only)
  schools: any[] = [];
  showSchoolDropdown = false;

  form = this.fb.group({
    fullName: ['', Validators.required],
    userName: ['', Validators.required],
    password: [''],
    role: ['', Validators.required],
    schoolId: [null as number | null],
    email: ['', [Validators.required, Validators.email]],
    phone: ['']
  });

  ngOnInit(): void {

    // Logged in user info
    const user = this.auth.getUserInfo();
    this.loggedInUserId = user?.userId.toString() ?? '';
    this.loggedInRole = user?.role;
    this.loggedInSchoolId = user?.schoolId;

    // Only SuperAdmin can assign school
    if (this.loggedInRole === 'SuperAdmin') {
      this.showSchoolDropdown = true;
      this.loadSchools(); // future API
    }

    // Edit mode
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.userId) {
      this.isEdit = true;
      this.loadUser();
    }
  }

  // ==============================
  // Load Schools (placeholder)
  // ==============================
  loadSchools() {
    this.schoolService.getAllSchools(this.loggedInRole, this.loggedInSchoolId).subscribe(data => {
      this.schools = data;
    });
  }

  // ==============================
  // Load User for Edit
  // ==============================
  loadUser() {
    this.userService.getUserById(this.userId).subscribe(user => {
      this.form.patchValue({
        fullName: user.fullName,
        userName: user.userName,
        role: user.role,
        schoolId: user.schoolId,
        email: user.email,
        phone: user.phone
      });
    });
  }

  // ==============================
  // Save
  // ==============================
  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request: User = {
      userId: this.isEdit ? this.userId : 0,
      fullName: this.form.value.fullName ?? '',
      userName: this.form.value.userName ?? '',
      password: this.form.value.password ?? '',
      role: this.form.value.role ?? '',
      schoolId: this.form.value.schoolId ?? null,
      email: this.form.value.email ?? '',
      phone: this.form.value.phone ?? '',
      updatedBy: this.loggedInUserId
    };

    this.userService.manageUser(request).subscribe(() => {
      this.router.navigate(['/users']);
    });
  }

  cancel() {
    this.router.navigate(['/users']);
  }
}
