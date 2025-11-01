import { Application, Department, Employee } from '../types';
import dayjs from 'dayjs';
import classNames from 'classnames';

interface ApplicationCardProps {
  application: Application;
  departments?: Department[];
  employees?: Employee[];
}

const statusColors: Record<Application['status'], string> = {
  draft: 'bg-slate-200 text-slate-700',
  pending: 'bg-amber-200 text-amber-800',
  approved: 'bg-emerald-200 text-emerald-800',
  rejected: 'bg-red-200 text-red-800',
};

export const ApplicationCard: React.FC<ApplicationCardProps> = ({ application, departments, employees }) => {
  const departmentName = departments?.find((d) => d.id === application.departmentId)?.name;
  const employeeName = employees?.find((e) => e.id === application.employeeId)?.fullName;

  return (
    <article className="flex flex-col gap-4 rounded-xl border border-slate-200 bg-white p-5 shadow-sm">
      <header className="flex items-center justify-between">
        <div>
          <p className="text-xs uppercase tracking-wide text-slate-400">
            {application.type === 'personal' ? 'ЛИЧНАЯ' : 'ГРУППОВАЯ'} ЗАЯВКА
          </p>
          <h3 className="text-lg font-semibold text-slate-800">{application.purpose}</h3>
        </div>
        <span
          className={classNames(
            'rounded-full px-3 py-1 text-xs font-semibold',
            statusColors[application.status]
          )}
        >
          {application.status === 'pending'
            ? 'На рассмотрении'
            : application.status === 'approved'
            ? 'Одобрена'
            : application.status === 'rejected'
            ? 'Отклонена'
            : 'Черновик'}
        </span>
      </header>
      <div className="grid gap-3 text-sm text-slate-600 md:grid-cols-2">
        <p>
          <span className="font-medium text-slate-500">Период:</span>{' '}
          {dayjs(application.startDate).format('DD.MM.YYYY')} — {dayjs(application.endDate).format('DD.MM.YYYY')}
        </p>
        <p>
          <span className="font-medium text-slate-500">Подразделение:</span> {departmentName || '—'}
        </p>
        <p>
          <span className="font-medium text-slate-500">Принимающий сотрудник:</span> {employeeName || '—'}
        </p>
        <p>
          <span className="font-medium text-slate-500">Дата подачи:</span>{' '}
          {dayjs(application.createdAt).format('DD.MM.YYYY HH:mm')}
        </p>
        <p>
          <span className="font-medium text-slate-500">Контакты:</span> {application.applicantPhone}
        </p>
        <p>
          <span className="font-medium text-slate-500">Паспорт:</span> {application.applicantPassport}
        </p>
        {application.type === 'group' && (
          <p>
            <span className="font-medium text-slate-500">Количество участников:</span> {application.groupSize ?? '—'}
          </p>
        )}
      </div>
    </article>
  );
};
