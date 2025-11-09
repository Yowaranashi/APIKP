import axiosClient from './axiosClient';

export interface HealthStatus {
  api: string;
  database: boolean;
  timestamp: string;
  message?: string;
}

export const checkHealth = async () => {
  const { data } = await axiosClient.get<HealthStatus>('/api/health/ping');
  return data;
};

export default checkHealth;
