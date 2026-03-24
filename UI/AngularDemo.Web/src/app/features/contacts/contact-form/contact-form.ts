import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Validators, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ContactsService } from '../contacts';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatCheckboxModule],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.css',
})
export class ContactForm {

  isEditMode = false;
  contactId: string | null = null;

  private contactsService = inject(ContactsService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  form = new FormGroup({
    name: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$')]),
    favorite: new FormControl(false)
  });

  ngOnInit() {
    this.contactId = this.route.snapshot.paramMap.get('id');

    if (this.contactId) {
      this.isEditMode = true;

      this.contactsService.getContactById(this.contactId)
        .subscribe(contact => {
          this.form.patchValue(contact);
        });
    }
  }


  submit() {
    debugger
    if(this.form.invalid) {
      debugger
      this.form.markAllAsTouched();
      return;
    }

    if (this.form.valid) {
      if (this.isEditMode && this.contactId) {
        this.contactsService.updateContact(this.contactId, this.form.value) .subscribe(() => {
          this.router.navigate(['/contacts']);
        });
      }
      else {
        this.contactsService.addContact(this.form.value).subscribe(() => {
          this.router.navigate(['/contacts']);
        })
      }
    }
  }

  cancel() {
    this.router.navigate(['/contacts']);
  }
}
