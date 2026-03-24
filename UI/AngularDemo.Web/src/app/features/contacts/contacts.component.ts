import { Component, inject, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ContactsService } from './contacts';
import { Contacts } from '../../models/contact';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialog } from '../../shared/confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-contacts',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatIconModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatPaginatorModule, MatSortModule, MatDialogModule],
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})

export class ContactsComponent implements OnInit {

  contacts: any[] = [];   // initialize empty array

  displayedColumns: string[] = ['name', 'email', 'phone', 'favorite', 'actions'];
  dataSource = new MatTableDataSource<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private contactsService = inject(ContactsService);
  private dialog = inject(MatDialog)

  contactsForm = new FormGroup({
    name: new FormControl<string>(''),
    email: new FormControl<string | null>(''),
    phone: new FormControl<string>(''),
    favorite: new FormControl<boolean>(false)
  });

  ngOnInit() {
    this.loadContacts();
  }

  loadContacts() {
    this.contactsService.getContacts().subscribe(res => {
      this.dataSource.data = res;

      setTimeout(() => {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });

      // this.cdr.detectChanges();   // force UI update
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onFormSubmit() {
    const request = {
      name: this.contactsForm.value.name!,
      email: this.contactsForm.value.email,
      phone: this.contactsForm.value.phone!,
      favorite: this.contactsForm.value.favorite ?? false
    };

    this.contactsService.addContact(request).subscribe(() => {
      this.loadContacts();
      this.contactsForm.reset();
    });
  }

  onDelete(id: string) {
    this.contactsService.deleteContact(id).subscribe(() => {
      this.loadContacts();
    });
  }

  openAddContact() {
    this.router.navigate(['/contacts/add']);
  }

  onEdit(id: string) {
    this.router.navigate(['/contacts/edit', id]);
  }

  deleteContact(id: string) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '350px',
      data: {
        title: 'Delete Contact',
        message: 'Are you sure you want to delete this contact?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.contactsService.deleteContact(id).subscribe(() => {
          this.loadContacts();
        });
      }
    });
  }
}
