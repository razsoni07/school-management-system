import { Component, OnInit, HostListener, ElementRef } from '@angular/core';
import { Router, RouterOutlet, RouterModule, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from '../services/auth';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit {
  userName: string = '';
  userFullName: string = '';
  userRole: string = '';
  userSchoolName: string = '';
  currentYear = new Date().getFullYear();

  isDarkMode = false;
  isProfileOpen = false;
  isSidebarCollapsed = false;

  constructor(private router: Router, private auth: Auth, private eRef: ElementRef) {
    const savedTheme = localStorage.getItem('theme');

    if (savedTheme === 'dark') {
      document.body.classList.add('dark-theme');
      this.isDarkMode = true;
    } else {
      document.body.classList.add('light-theme');
    }
  }

  ngOnInit() {
    const user = this.auth.getUserInfo();

    if (user) {
      this.userName = user.username;
      this.userFullName = user.fullName;
      this.userRole = user.role;
      this.userSchoolName = user.schoolName;
    }

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        if (event.urlAfterRedirects.includes('/meetings/join/')) {
          this.isSidebarCollapsed = true;
        }
      });
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  toggleProfile() {
    this.isProfileOpen = !this.isProfileOpen;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (this.isProfileOpen && !this.eRef.nativeElement.querySelector('.profile').contains(event.target)) {
      this.isProfileOpen = false;
    }
  }

  toggleTheme() {
    this.isDarkMode = !this.isDarkMode;
    
    const body = document.body;
    
    if (this.isDarkMode) {
      document.body.classList.remove('light-theme');
      document.body.classList.add('dark-theme');
      localStorage.setItem('theme', 'dark');
    } else {
      document.body.classList.remove('dark-theme');
      document.body.classList.add('light-theme');
      localStorage.setItem('theme', 'light');
    }
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
