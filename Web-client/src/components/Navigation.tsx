import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import classNames from 'classnames';

export const Navigation = () => {
  const { isAuthenticated, logout, user } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

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
            </>
          )}
        </nav>
      </div>
    </header>
  );
};
