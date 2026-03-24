export interface Teacher {
  id?: number;
  fullName: string;
  userName: string;
  email: string;
  phone: string;
  userId: number;
  schoolId?: number;
  schoolName?: string;
  schoolCode?: string;
  employeeCode: string;
  departmentId?: number;
  qualification: string;
  experienceYears: number;
  joiningDate: string;
  subjects: string;
  classTeacherOf: string;
  isActive: boolean;
}
