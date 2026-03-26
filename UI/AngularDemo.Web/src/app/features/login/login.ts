import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  username = '';
  password = '';
  errorMessage = '';
  isMsLoading = false;

  constructor(private auth: Auth, private router: Router) {}

  onLogin() {
    this.auth.login(this.username, this.password).subscribe({
      next: (res) => {
        this.auth.saveToken(res.token);
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage = 'Invalid username or password';
      }
    });
  }

  loginWithMicrosoft() {
    debugger
    this.errorMessage = '';
    this.isMsLoading = true;

    this.auth.loginWithMicrosoft().subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage = 'Microsoft sign-in failed. Please try again.';
        this.isMsLoading = false;
      },
      complete: () => {
        // Handles silent cancellation (popup closed) — re-enable button, no error shown
        this.isMsLoading = false;
      }
    });
  }
}
