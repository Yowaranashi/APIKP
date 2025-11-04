import axiosClient from './axiosClient';
import { Department, Employee } from '../types';

interface ApiDepartmentDto {
  id: number;
  name: string;
}

interface ApiEmployeeDto {
  id: number;
  fullName: string;
  departmentId?: number | null;
}

export const getDepartments = async () => {
  const { data } = await axiosClient.get<ApiDepartmentDto[]>('/api/departments');
  return data.map<Department>((department) => ({
    id: department.id.toString(),
    name: department.name,
  }));
};

export const getEmployees = async (departmentId?: string) => {
  const params = departmentId ? { departmentId } : undefined;
  const { data } = await axiosClient.get<ApiEmployeeDto[]>('/api/users', { params });
  return data.map<Employee>((employee) => ({
    id: employee.id.toString(),
    fullName: employee.fullName,
    departmentId: employee.departmentId?.toString() ?? '',
  }));
};
