import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { ContactsComponent } from './features/contacts/contacts.component';
import { Layout } from './layout/layout';
import { ContactForm } from './features/contacts/contact-form/contact-form';
import { Users } from './features/users/users';
import { UserForm } from './features/users/user-form/user-form';
import { Dashboard } from './features/dashboard/dashboard';
import { Schools } from './features/schools/schools';
import { SchoolForm } from './features/schools/school-form/school-form';
import { Teachers } from './features/teachers/teachers';
import { TeacherForm } from './features/teachers/teacher-form/teacher-form';
import { Students } from './features/students/students';
import { StudentForm } from './features/students/student-form/student-form';
import { ScheduleMeeting } from './features/meetings/schedule-meeting/schedule-meeting';
import { Meetings } from './features/meetings/meetings';
import { JoinMeeting } from './features/meetings/join-meeting/join-meeting';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: Login },
    {
        path: '',
        component: Layout,
        children: [
            { path: 'dashboard', component: Dashboard },
            { path: 'contacts', component: ContactsComponent },
            { path: 'contacts/add', component: ContactForm },
            { path: 'contacts/edit/:id', component: ContactForm },
            { path: 'users', component: Users },
            { path: 'users/add', component: UserForm },
            { path: 'users/edit/:id', component: UserForm },
            { path: 'schools', component: Schools },
            { path: 'schools/add', component: SchoolForm },
            { path: 'schools/edit/:id', component: SchoolForm },
            { path: 'teachers', component: Teachers },
            { path: 'teachers/add', component: TeacherForm },
            { path: 'teachers/edit/:id', component: TeacherForm },
            { path: 'students', component: Students },
            { path: 'students/add', component: StudentForm },
            { path: 'students/edit/:id', component: StudentForm },
            {
              path: 'meetings',
              children: [
                { path: '', component: Meetings },
                { path: 'schedule-meeting', component: ScheduleMeeting },
                { path: 'join/:meetingId/:roomName', component: JoinMeeting }
              ]
            }
        ]
    },    
    { path: '**', redirectTo: 'login' }
];
