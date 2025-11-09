import axios from 'axios';

const apiUrl = import.meta.env.VITE_API_URL || 'http://185.68.246.157:5000';

export const axiosClient = axios.create({
  baseURL: apiUrl,
  withCredentials: false,
});

axiosClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('hrp_token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosClient;
