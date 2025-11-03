import { useEffect, useMemo, useState } from 'react';
import { Application, Department, Employee } from '../types';
import { getUserApplications } from '../api/applications';
import { getDepartments, getEmployees } from '../api/directory';
import { useAuth } from '../hooks/useAuth';
import { ApplicationCard } from '../components/ApplicationCard';
import { Link } from 'react-router-dom';
import toast from 'react-hot-toast';
import { demoApplications, demoDepartments, demoEmployees } from '../utils/demoData';

export const Dashboard = () => {
  const { user } = useAuth();
  const [applications, setApplications] = useState<Application[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        const [apps, deps, emps] = await Promise.all([
          user ? getUserApplications(user.id) : Promise.resolve([]),
          getDepartments(),
          getEmployees(),
        ]);
        setApplications(apps);
        setDepartments(deps);
        setEmployees(emps);
      } catch (error: any) {
        const message = error?.response?.data?.message || 'Не удалось загрузить данные';
        toast.error(`${message}. Показаны демонстрационные данные.`);
        setApplications(demoApplications);
        setDepartments(demoDepartments);
        setEmployees(demoEmployees);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [user]);

  const hasApplications = useMemo(() => applications.length > 0, [applications]);

  return (
    <section className="flex flex-col gap-6">
      <header className="flex flex-col justify-between gap-4 rounded-xl bg-gradient-to-r from-primary to-blue-600 p-6 text-white md:flex-row md:items-center">
        <div>
          <h1 className="text-2xl font-semibold">Добро пожаловать, {user?.fullName}!</h1>
          <p className="mt-1 max-w-xl text-sm text-blue-100">
            Управляйте заявками на посещение объекта КИИ, отслеживайте статусы и оформляйте новые пропуска для гостей и
            экскурсионных групп.
          </p>
        </div>
        <div className="flex flex-wrap gap-3">
          <Link
            to="/applications/personal"
            className="rounded-md bg-white/90 px-4 py-2 text-sm font-semibold text-primary shadow-sm transition hover:bg-white"
          >
            Личная заявка
          </Link>
          <Link
            to="/applications/group"
            className="rounded-md bg-secondary px-4 py-2 text-sm font-semibold text-white shadow-sm transition hover:bg-secondary/90"
          >
            Групповая заявка
          </Link>
        </div>
      </header>

      {loading ? (
        <div className="py-20 text-center text-slate-500">Загрузка заявок...</div>
      ) : hasApplications ? (
        <div className="grid gap-4">
          {applications.map((application) => (
            <ApplicationCard
              key={application.id}
              application={application}
              departments={departments}
              employees={employees}
            />
          ))}
        </div>
      ) : (
        <div className="flex flex-col items-center justify-center gap-4 rounded-xl border border-dashed border-slate-300 bg-white p-10 text-center text-slate-500">
          <p className="text-lg font-medium text-slate-600">У вас пока нет заявок</p>
          <p className="max-w-md text-sm">
            Оформите первую заявку на посещение, чтобы начать процесс согласования. Вы можете выбрать личное или групповое посещение.
          </p>
          <div className="flex flex-wrap justify-center gap-3">
            <Link to="/applications/personal" className="rounded-md bg-primary px-4 py-2 text-sm font-semibold text-white">
              Создать личную
            </Link>
            <Link to="/applications/group" className="rounded-md border border-primary px-4 py-2 text-sm font-semibold text-primary">
              Создать групповую
            </Link>
          </div>
        </div>
      )}
    </section>
  );
};
