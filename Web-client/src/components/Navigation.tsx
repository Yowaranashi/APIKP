import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useCallback, useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import classNames from 'classnames';
import toast from 'react-hot-toast';
import { checkHealth } from '../api/health';

export const Navigation = () => {
  const { isAuthenticated, logout, user } = useAuth();
  const navigate = useNavigate();
  const [checkingHealth, setCheckingHealth] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const handleCheckApi = useCallback(async () => {
    setCheckingHealth(true);
    try {
      const status = await checkHealth();
      const databaseMessage = status.database ? 'База данных доступна' : 'База данных недоступна';

      if (status.api?.toLowerCase() === 'ok') {
        const toastMessage = status.database
          ? 'API отвечает. База данных доступна.'
          : 'API отвечает, но база данных недоступна.';
        status.database ? toast.success(toastMessage) : toast.error(toastMessage);
      } else {
        toast.error('API ответил ошибкой. Проверьте подключение.');
      }

      console.info('[health-check]', {
        api: status.api,
        database: status.database,
        timestamp: status.timestamp,
        message: status.message,
        details: databaseMessage,
      });
    } catch (error) {
      const message =
        (error as any)?.response?.data?.message ?? (error as Error)?.message ?? 'Не удалось связаться с API';
      toast.error(message);
      console.error('[health-check:error]', error);
    } finally {
      setCheckingHealth(false);
    }
  }, []);

  const navLinkClass = ({ isActive }: { isActive: boolean }) =>
    classNames(
      'rounded-md px-4 py-2 text-sm font-medium transition-colors',
      isActive
        ? 'bg-primary text-white shadow'
        : 'text-primary hover:bg-primary/10 hover:text-primary'
    );

  return (
    <header className="bg-white shadow-sm">
      <div className="mx-auto flex w-full max-w-6xl items-center justify-between px-4 py-4">
        <Link to={isAuthenticated ? '/dashboard' : '/'} className="flex items-center gap-2">
          <span className="h-10 w-10 rounded-full bg-primary text-white grid place-items-center font-bold">HP</span>
          <div>
            <p className="text-lg font-semibold text-primary">ХранительПРО</p>
            <p className="text-xs text-slate-500">Контроль доступа КИИ</p>
          </div>
        </Link>
        <nav className="flex items-center gap-2">
          {isAuthenticated ? (
            <>
              <NavLink to="/dashboard" className={navLinkClass}>
                Мои заявки
              </NavLink>
              <NavLink to="/applications/personal" className={navLinkClass}>
                Личная заявка
              </NavLink>
              <NavLink to="/applications/group" className={navLinkClass}>
                Групповая заявка
              </NavLink>
              <button
                type="button"
                onClick={handleCheckApi}
                disabled={checkingHealth}
                className="rounded-md border border-primary px-3 py-1 text-sm font-medium text-primary transition hover:bg-primary/10 disabled:cursor-not-allowed disabled:opacity-60"
              >
                {checkingHealth ? 'Проверяем API…' : 'Проверить API'}
              </button>
              <div className="flex items-center gap-3 pl-4 text-sm text-slate-600">
                <span className="hidden sm:inline-block">{user?.fullName}</span>
                <button
                  onClick={handleLogout}
                  className="rounded-md border border-primary px-3 py-1 text-primary transition hover:bg-primary hover:text-white"
                >
                  Выйти
                </button>
              </div>
            </>
          ) : (
            <>
              <NavLink to="/login" className={navLinkClass}>
                Вход
              </NavLink>
              <NavLink to="/register" className={navLinkClass}>
                Регистрация
              </NavLink>
              <button
                type="button"
                onClick={handleCheckApi}
                disabled={checkingHealth}
                className="rounded-md border border-primary px-3 py-1 text-sm font-medium text-primary transition hover:bg-primary/10 disabled:cursor-not-allowed disabled:opacity-60"
              >
                {checkingHealth ? 'Проверяем API…' : 'Проверить API'}
              </button>
            </>
          )}
        </nav>
      </div>
    </header>
  );
};
