import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { SchoolService } from '../../services/school-service';
import { School } from '../../models/school';
import { Auth } from '../../services/auth';

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
  selector: 'app-schools',
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
  templateUrl: './schools.html',
  styleUrl: './schools.css',
})
export class Schools implements OnInit {
  schools: School[] = [];
  loggedInRole = '';
  loggedInSchoolId?: number;

  // Updated columns (match backend fields)
  displayedColumns: string[] = [
    'schoolName',
    'code',
    'address',
    'city',
    'state',
    'createdDate',
    'isActive',
    'actions'
  ];

  dataSource = new MatTableDataSource<School>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private schoolService: SchoolService,
    private router: Router,
    private dialog: MatDialog,
    private auth: Auth
  ) {}

  ngOnInit(): void {
    const user = this.auth.getUserInfo();
    this.loggedInRole = user?.role ?? '';
    this.loggedInSchoolId = user?.schoolId;
    this.loadSchools();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  loadSchools() {    
    this.schoolService.getAllSchools(this.loggedInRole, this.loggedInSchoolId).subscribe({
      next: (res: School[]) => {
        debugger
        this.schools = res;
        this.dataSource.data = res;
      },
      error: (err) => console.error(err)
    })
  }

  addSchool() {
    this.router.navigate(['/schools/add']);
  }

  editSchool(id: number) {
    this.router.navigate(['/schools/edit', id]);
  }

  deleteSchool(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: "350px",
      data: {
        title: 'Delete School',
        message: 'Are you sure you want to delete this school?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      debugger
      if(result) {
        debugger
        this.schoolService.deleteSchool(id).subscribe(() => {
          debugger
          this.loadSchools();
        })
      }
    })
  }

  toggleStatus(school: any) {
    debugger
    this.schoolService.toggleSchoolStatus(school.id).subscribe({
        next: (res) => {
          school.isActive = !school.isActive;
          console.log(res.message);
        },
        error: (err) => {
          console.error('Error updating status', err);
        }
      });
  }
}
