export interface School {
  Id?: number;
  schoolName: string;
  code: string;
  address?: string;
  city: string;
  state?: string;
  isActive?: boolean;
  createdBy?: string;
  createdDate?: string;
  updatedBy?: string;
  updatedDate?: string;
}
