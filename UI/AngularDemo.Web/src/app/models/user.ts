export interface User {
  userId?: number;
  fullName: string;
  userName: string;
  password?: string;
  role: string;
  email?: string;
  phone?: string;
  schoolId?: number | null;
  schoolName?: string;
  isActive?: boolean;
  createdDate?: string;
  createdBy?: string;
  updatedDate?: string;
  updatedBy?: string;
}
