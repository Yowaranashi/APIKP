import React, { createContext, useContext, useEffect, useMemo, useState } from 'react';
import { AuthResponse, User } from '../types';
import * as authApi from '../api/auth';
import toast from 'react-hot-toast';

type AuthContextValue = {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  login: (payload: authApi.LoginPayload) => Promise<void>;
  register: (payload: authApi.RegisterPayload) => Promise<void>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const TOKEN_KEY = 'hrp_token';
const USER_KEY = 'hrp_user';

type AuthProviderProps = {
  children: React.ReactNode;
};

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem(TOKEN_KEY);
    const storedUser = localStorage.getItem(USER_KEY);
    if (storedToken && storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser) as User;
        setUser(parsedUser);
        setToken(storedToken);
      } catch (error) {
        console.error('Не удалось восстановить пользователя', error);
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
      }
    }
    setLoading(false);
  }, []);

  const handleAuthSuccess = (data: AuthResponse) => {
    setUser(data.user);
    setToken(data.token);
    localStorage.setItem(TOKEN_KEY, data.token);
    localStorage.setItem(USER_KEY, JSON.stringify(data.user));
  };

  const login = async (payload: authApi.LoginPayload) => {
    try {
      setLoading(true);
      const data = await authApi.login(payload);
      handleAuthSuccess(data);
      toast.success('Добро пожаловать в ХранительПРО!');
    } catch (error: any) {
      const message = error?.response?.data?.message || 'Не удалось войти. Проверьте данные.';
      toast.error(message);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const register = async (payload: authApi.RegisterPayload) => {
    try {
      setLoading(true);
      await authApi.register(payload);
      const authData = await authApi.login({ email: payload.email, password: payload.password });
      handleAuthSuccess(authData);

      toast.success('Регистрация прошла успешно!');
    } catch (error: any) {
      const message = error?.response?.data?.message || 'Регистрация не удалась.';
      toast.error(message);
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    toast.success('Вы вышли из системы');
  };

  const value = useMemo(
    () => ({
      user,
      token,
      isAuthenticated: Boolean(user && token),
      loading,
      login,
      register,
      logout,
    }),
    [user, token, loading]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuthContext = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuthContext должен вызываться внутри AuthProvider');
  }
  return context;
};
