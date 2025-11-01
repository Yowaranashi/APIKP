import axiosClient from './axiosClient';
import { AuthResponse } from '../types';

export interface RegisterPayload {
  email: string;
  password: string;
  confirmPassword: string;
  fullName: string;
}

export interface LoginPayload {
  email: string;
  password: string;
}

export const register = async (payload: RegisterPayload) => {
  const { data } = await axiosClient.post<AuthResponse>('/api/auth/register', payload);
  return data;
};

export const login = async (payload: LoginPayload) => {
  const { data } = await axiosClient.post<AuthResponse>('/api/auth/login', payload);
  return data;
};
