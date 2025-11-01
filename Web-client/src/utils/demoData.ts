import { Application, Department, Employee } from '../types';

export const demoDepartments: Department[] = [
  { id: 'dep-1', name: 'Служба безопасности' },
  { id: 'dep-2', name: 'ИТ-департамент' },
  { id: 'dep-3', name: 'Отдел эксплуатации' },
];

export const demoEmployees: Employee[] = [
  { id: 'emp-1', departmentId: 'dep-1', fullName: 'Иванова Мария Сергеевна' },
  { id: 'emp-2', departmentId: 'dep-2', fullName: 'Петров Алексей Андреевич' },
  { id: 'emp-3', departmentId: 'dep-3', fullName: 'Сидоров Николай Владимирович' },
];

export const demoApplications: Application[] = [
  {
    id: 'app-1',
    type: 'personal',
    purpose: 'Встреча с руководством по вопросам кибербезопасности',
    departmentId: 'dep-1',
    employeeId: 'emp-1',
    startDate: new Date().toISOString(),
    endDate: new Date(Date.now() + 2 * 24 * 60 * 60 * 1000).toISOString(),
    status: 'approved',
    createdAt: new Date().toISOString(),
    applicantName: 'Смирнов Андрей Владимирович',
    applicantPhone: '+7 (900) 123-45-67',
    applicantBirthDate: '1987-06-12',
    applicantPassport: '4001 234567',
  },
  {
    id: 'app-2',
    type: 'group',
    purpose: 'Экскурсия студентов кафедры информатики',
    departmentId: 'dep-2',
    employeeId: 'emp-2',
    startDate: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000).toISOString(),
    endDate: new Date(Date.now() + 5 * 24 * 60 * 60 * 1000).toISOString(),
    status: 'pending',
    createdAt: new Date().toISOString(),
    applicantName: 'Кузнецова Анна Игоревна',
    applicantPhone: '+7 (921) 555-66-77',
    applicantBirthDate: '1992-03-04',
    applicantPassport: '4012 765432',
    groupSize: 12,
  },
];
