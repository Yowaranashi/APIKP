import axiosClient from './axiosClient';
import { Department, Employee } from '../types';

export const getDepartments = async () => {
  const { data } = await axiosClient.get<Department[]>('/api/departments');
  return data;
};

export const getEmployees = async (departmentId?: string) => {
  const params = departmentId ? { departmentId } : undefined;
  const { data } = await axiosClient.get<Employee[]>('/api/users', { params });
  return data;
};
