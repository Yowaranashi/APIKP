import axiosClient from './axiosClient';
import { AuthResponse } from '../types';

export interface RegisterPayload {
  email: string;
  password: string;
  fullName: string;
  roleId: number;
}

export interface LoginPayload {
  email: string;
  password: string;
}

export interface RegisterResponse {
  message: string;
}

const normalizeEmail = (email: string) => email.trim().toLowerCase();

export const register = async (payload: RegisterPayload) => {
  const requestBody = {
    ...payload,
    email: payload.email.trim(),
  };
  const { data } = await axiosClient.post<RegisterResponse>('/api/auth/register', requestBody);
  return data;
};

export const login = async (payload: LoginPayload) => {
  const requestBody = {
    ...payload,
    email: payload.email.trim(),
  };
  const { data } = await axiosClient.post<AuthResponse>('/api/auth/login', requestBody);
  return data;
};
