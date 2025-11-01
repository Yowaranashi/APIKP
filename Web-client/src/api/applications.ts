import axiosClient from './axiosClient';
import { Application, ApplicationType } from '../types';

export interface ApplicationPayload {
  type: ApplicationType;
  startDate: string;
  endDate: string;
  purpose: string;
  departmentId: string;
  employeeId: string;
  applicantName: string;
  applicantPhone: string;
  applicantEmail?: string;
  applicantBirthDate: string;
  applicantPassport: string;
  applicantCompany?: string;
  groupSize?: number;
  files?: {
    photo?: File;
    passport?: File;
    participants?: File;
  };
  participants?: Array<{
    fullName: string;
    birthDate: string;
    passport: string;
  }>;
}

export const submitApplication = async (payload: ApplicationPayload) => {
  const formData = new FormData();
  Object.entries(payload).forEach(([key, value]) => {
    if (value === undefined || value === null) {
      return;
    }

    if (key === 'files' && typeof value === 'object') {
      const files = value as ApplicationPayload['files'];
      if (files?.photo) {
        formData.append('photo', files.photo);
      }
      if (files?.passport) {
        formData.append('passport', files.passport);
      }
      if (files?.participants) {
        formData.append('participants', files.participants);
      }
      return;
    }

    if (key === 'participants' && Array.isArray(value)) {
      formData.append('participantsJson', JSON.stringify(value));
      return;
    }

    if (typeof value === 'number') {
      formData.append(key, value.toString());
      return;
    }

    formData.append(key, value as string);
  });

  const { data } = await axiosClient.post<Application>('/api/applications', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
  return data;
};

export const getUserApplications = async (userId: string) => {
  const { data } = await axiosClient.get<Application[]>(`/api/applications`, {
    params: { userId },
  });
  return data;
};
