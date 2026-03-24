import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SchoolService } from '../../../services/school-service';
import { Auth } from '../../../services/auth';
import { School } from '../../../models/school';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-school-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule],
  templateUrl: './school-form.html',
  styleUrl: './school-form.css',
})
export class SchoolForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private schoolService = inject(SchoolService);
  private auth = inject(Auth);

  schoolId = 0;
  isEdit = false;
  loggedInUserId = '';
  loggedInRole = '';

  form = this.fb.group({
    schoolName: ['', Validators.required],
    code: ['', Validators.required],
    address: [''],
    city: ['', Validators.required],
    state: ['']
  });

  ngOnInit(): void {
    // Logged in user info
    const user = this.auth.getUserInfo();
    this.loggedInUserId = user?.userId.toString() ?? '';
    this.loggedInRole = user?.role;

    // Edit mode
    this.schoolId = Number(this.route.snapshot.paramMap.get('id'));
    debugger
    if (this.schoolId) {
      this.isEdit = true;
      this.loadSchool();
    }
  }

  // ==============================
  // Load School for Edit
  // ==============================
  loadSchool() {
    this.schoolService.getSchoolById(this.schoolId).subscribe(school => {
      this.form.patchValue({
        schoolName: school.schoolName,
        code: school.code,
        address: school.address,
        city: school.city,
        state: school.state
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

    debugger
    const request: School = {      
      Id: this.isEdit ? this.schoolId : 0,
      schoolName: this.form.value.schoolName ?? '',
      code: this.form.value.code ?? '',
      address: this.form.value.address ?? '',
      city: this.form.value.city ?? '',
      state: this.form.value.state ?? ''
    };

    this.schoolService.manageSchool(request).subscribe(() => {
      this.router.navigate(['/schools']);
    });
  }

  cancel() {
    this.router.navigate(['/schools']);
  }
}
