import { Navigate, Route, Routes } from 'react-router-dom';
import { LoginPage } from '../pages/LoginPage';
import { RegisterPage } from '../pages/RegisterPage';
import { Dashboard } from '../pages/Dashboard';
import { ApplicationFormPersonal } from '../pages/ApplicationFormPersonal';
import { ApplicationFormGroup } from '../pages/ApplicationFormGroup';
import { useAuth } from '../hooks/useAuth';
import { useEffect } from 'react';

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated, loading } = useAuth();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  if (loading) {
    return <div className="py-20 text-center text-slate-500">Загрузка...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

const GuestRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <Navigate to="/dashboard" replace /> : children;
};

const AppRouter = () => {
  return (
    <Routes>
      <Route
        path="/"
        element={<Navigate to="/dashboard" replace />}
      />
      <Route
        path="/login"
        element={
          <GuestRoute>
            <LoginPage />
          </GuestRoute>
        }
      />
      <Route
        path="/register"
        element={
          <GuestRoute>
            <RegisterPage />
          </GuestRoute>
        }
      />
      <Route
        path="/dashboard"
        element={
          <ProtectedRoute>
            <Dashboard />
          </ProtectedRoute>
        }
      />
      <Route
        path="/applications/personal"
        element={
          <ProtectedRoute>
            <ApplicationFormPersonal />
          </ProtectedRoute>
        }
      />
      <Route
        path="/applications/group"
        element={
          <ProtectedRoute>
            <ApplicationFormGroup />
          </ProtectedRoute>
        }
      />
      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
};

export default AppRouter;
