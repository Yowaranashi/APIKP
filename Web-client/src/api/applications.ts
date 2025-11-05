import axiosClient from './axiosClient';
import { Application, ApplicationType } from '../types';

export interface ApplicationPayload {
  type: ApplicationType;
  startDate: string;
  endDate: string;
  purpose: string;
  departmentId: number;
  employeeId: number;
  applicantName: string;
  applicantPhone: string;
  applicantEmail?: string;
  applicantBirthDate: string;
  applicantPassport: string;
  applicantCompany?: string;
  groupSize?: number;
  applicantUserId?: number;
  note?: string;
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

type ApiApplicationStatus = 'Pending' | 'Approved' | 'Rejected' | 'Draft' | string;

interface ApiPassRequestSummary {
  id: number;
  requestType?: string;
  department?: string;
  departmentId: number;
  responsibleEmployeeId?: number;
  startDate: string;
  endDate: string;
  status: ApiApplicationStatus;
  purpose: string;
  visitorCount: number;
  groupSize?: number | null;
  createdAt: string;
  applicantFullName?: string;
  applicantPhone?: string;
  applicantBirthDate?: string | null;
  applicantPassport?: string;
  applicantEmail?: string;
  applicantOrganization?: string;
}

const mapStatus = (status: ApiApplicationStatus): Application['status'] => {
  switch (status?.toLowerCase()) {
    case 'approved':
      return 'approved';
    case 'rejected':
      return 'rejected';
    case 'pending':
      return 'pending';
    default:
      return 'draft';
  }
};

const mapToApplication = (item: ApiPassRequestSummary): Application => ({
  id: item.id.toString(),
  type: item.requestType?.toLowerCase() === 'group' ? 'group' : 'personal',
  purpose: item.purpose,
  departmentId: item.departmentId?.toString() ?? '',
  employeeId: item.responsibleEmployeeId?.toString() ?? '',
  startDate: item.startDate,
  endDate: item.endDate,
  status: mapStatus(item.status),
  createdAt: item.createdAt,
  applicantName: item.applicantFullName || '',
  applicantPhone: item.applicantPhone || '',
  applicantBirthDate: item.applicantBirthDate || '',
  applicantPassport: item.applicantPassport || '',
  files: [],
  groupSize:
    item.requestType?.toLowerCase() === 'group'
      ? item.groupSize ?? item.visitorCount ?? undefined
      : undefined,
});

export const getUserApplications = async (userId: number) => {
  const { data } = await axiosClient.get<ApiPassRequestSummary[]>(`/api/applications`, {
    params: { userId: userId.toString() },
  });
  return data.map(mapToApplication);
};
