import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { StudentService } from '../../services/student-service';
import { Auth } from '../../services/auth';
import { Student } from '../../models/student';

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

@Component({
  selector: 'app-students',
  standalone: true,
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
  templateUrl: './students.html',
  styleUrl: './students.css',
})
export class Students implements OnInit {
  students: Student[] = [];
  loggedInRole = '';
  loggedInSchoolId?: number;

  // Base columns visible to all roles
  private baseColumns: string[] = [
    'fullName',
    'class',
    'section',
    'rollNumber',
    'gender',
    'admissionDate',
    'email',
    'phone',
    'isActive',
    'actions'
  ];

  // Extra columns only for SuperAdmin
  private superAdminColumns: string[] = [
    'fullName',
    'class',
    'section',
    'rollNumber',
    'schoolName',
    'schoolCode',
    'gender',
    'admissionDate',
    'email',
    'phone',
    'isActive',
    'actions'
  ];

  displayedColumns: string[] = [];

  dataSource = new MatTableDataSource<Student>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private studentService: StudentService,
    private auth: Auth,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    const user = this.auth.getUserInfo();
    this.loggedInRole = user?.role ?? '';
    this.loggedInSchoolId = user?.schoolId;
    this.displayedColumns = this.loggedInRole === 'SuperAdmin' ? this.superAdminColumns : this.baseColumns;
    this.loadStudents();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  loadStudents() {
    this.studentService.getAllStudents(this.loggedInRole, this.loggedInSchoolId).subscribe({      
      next: (res: Student[]) => {
        debugger;
        this.students = res;
        this.dataSource.data = res;
      },
      error: (err) => console.error(err)
    });
  }

  addStudent() {
    this.router.navigate(['/students/add']);
  }

  editStudent(id: number) {
    this.router.navigate(['/students/edit', id]);
  }

  deleteStudent(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '350px',
      data: {
        title: 'Delete Student',
        message: 'Are you sure you want to delete this student?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.studentService.deleteStudent(id, this.loggedInRole).subscribe(() => {
          this.loadStudents();
        });
      }
    });
  }

  toggleStatus(student: any) {
    this.studentService.toggleStudentStatus(student.id).subscribe({
      next: (res) => {
        student.isActive = !student.isActive;
        console.log(res.message);
      },
      error: (err) => {
        console.error('Error updating status', err);
      }
    });
  }
}
