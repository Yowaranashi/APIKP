export type ApplicationType = 'personal' | 'group';

export interface User {
  id: number;
  email: string;
  fullName: string;
  role: string;
}

export interface Department {
  id: string;
  name: string;
}

export interface Employee {
  id: string;
  fullName: string;
  departmentId: string;
}

export interface ApplicationFile {
  id?: string;
  type: 'photo' | 'passport' | 'participants';
  url?: string;
  file?: File;
  name?: string;
}

export interface Application {
  id: string;
  type: ApplicationType;
  purpose: string;
  departmentId: string;
  employeeId: string;
  startDate: string;
  endDate: string;
  status: 'draft' | 'pending' | 'approved' | 'rejected';
  createdAt: string;
  applicantName: string;
  applicantPhone: string;
  applicantBirthDate: string;
  applicantPassport: string;
  files?: ApplicationFile[];
  groupSize?: number;
}

export interface AuthResponse {
  token: string;
  user: User;
}
