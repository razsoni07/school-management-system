import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { TeacherService } from '../../services/teacher-service';
import { Auth } from '../../services/auth';
import { Teacher } from '../../models/teacher';

@Component({
  selector: 'app-teachers',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule
  ],
  templateUrl: './teachers.html',
  styleUrl: './teachers.css',
})
export class Teachers implements OnInit {
  teachers: Teacher[] = [];
  loggedInRole = '';
  loggedInSchoolId?: number;

  displayedColumns: string[] = [
    'fullName',
    'employeeCode',
    'email',
    'phone',
    'department',
    'qualification',
    'schoolName',
    'schoolCode',
    'actions'
  ];
  dataSource = new MatTableDataSource<Teacher>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private teacherService: TeacherService,
    private auth: Auth,
    private router: Router
  ) {}

  ngOnInit() {
    const user = this.auth.getUserInfo();
    this.loggedInRole = user?.role ?? '';
    this.loggedInSchoolId = user?.schoolId;
    this.loadTeachers();
  }

  loadTeachers() {
    this.teacherService.getTeachers(this.loggedInRole, this.loggedInSchoolId).subscribe({
      next: (data: Teacher[]) => {
        this.teachers = data;
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => console.error(err)
    });
  }

  addTeacher() {
    this.router.navigate(['/teachers/add']);
  }

  editTeacher(id: number) {
    debugger
    this.router.navigate(['/teachers/edit', id]);
  }

  deleteTeacher(id: number) {
    debugger
    this.teacherService.deleteTeacher(id).subscribe(() => {
      this.loadTeachers();
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
