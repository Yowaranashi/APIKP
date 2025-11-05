import { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FormInput } from '../components/FormInput';
import { FileUpload } from '../components/FileUpload';
import { Department, Employee } from '../types';
import { getDepartments, getEmployees } from '../api/directory';
import { submitApplication } from '../api/applications';
import { useAuth } from '../hooks/useAuth';
import toast from 'react-hot-toast';
import { isAdult, isDateRangeValid, isValidPassport, isValidPhone } from '../utils/validators';
import { formatPassport, formatPhone } from '../utils/formatters';
import { demoDepartments, demoEmployees } from '../utils/demoData';
import { useNavigate } from 'react-router-dom';

interface PersonalApplicationForm {
  startDate: string;
  endDate: string;
  purpose: string;
  departmentId: string;
  employeeId: string;
  applicantName: string;
  applicantPhone: string;
  applicantBirthDate: string;
  applicantPassport: string;
  photo: File | null;
  passportFile: File | null;
}

const schema: yup.ObjectSchema<PersonalApplicationForm> = yup.object({
  startDate: yup.string().required('Укажите дату начала'),
  endDate: yup.string().required('Укажите дату окончания'),
  purpose: yup.string().required('Опишите цель посещения'),
  departmentId: yup.string().required('Выберите подразделение'),
  employeeId: yup.string().required('Выберите сотрудника'),
  applicantName: yup.string().required('Укажите ФИО посетителя'),
  applicantPhone: yup
    .string()
    .required('Укажите телефон')
    .test('phone-mask', 'Формат телефона: +7 (XXX) XXX-XX-XX', isValidPhone),
  applicantBirthDate: yup
    .string()
    .required('Укажите дату рождения')
    .test('age', 'Возраст должен быть не менее 16 лет', (value) => (value ? isAdult(value) : false)),
  applicantPassport: yup
    .string()
    .required('Укажите паспортные данные')
    .test('passport', 'Формат: 1234 567890', isValidPassport),
  photo: yup
    .mixed<File>()
    .required('Загрузите фотографию')
    .test('file-type', 'Допускается только JPG/PNG', (file) => (file ? /image\/(jpeg|png)/.test(file.type) : false))
    .test('file-size', 'Размер файла до 4 МБ', (file) => (file ? file.size <= 4 * 1024 * 1024 : false)),
  passportFile: yup
    .mixed<File>()
    .required('Загрузите скан паспорта')
    .test('file-type', 'Допускается только PDF', (file) => (file ? file.type === 'application/pdf' : false))
    .test('file-size', 'Размер файла до 4 МБ', (file) => (file ? file.size <= 4 * 1024 * 1024 : false)),
});

export const ApplicationFormPersonal = () => {
  const { user } = useAuth();
  const [departments, setDepartments] = useState<Department[]>([]);
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const {
    register,
    control,
    handleSubmit,
    formState,
    setValue,
    watch,
  } = useForm<PersonalApplicationForm>({
    resolver: yupResolver(schema),
    defaultValues: {
      startDate: '',
      endDate: '',
      purpose: '',
      departmentId: '',
      employeeId: '',
      applicantName: user?.fullName || '',
      applicantPhone: '',
      applicantBirthDate: '',
      applicantPassport: '',
      photo: null,
      passportFile: null,
    },
  });

  const selectedDepartmentId = watch('departmentId');
  const phoneValue = watch('applicantPhone');
  const passportValue = watch('applicantPassport');

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        const [deps, emps] = await Promise.all([getDepartments(), getEmployees()]);
        setDepartments(deps);
        setEmployees(emps);
      } catch (error: any) {
        toast.error((error?.response?.data?.message || 'Не удалось загрузить справочники') + '. Используются демонстрационные данные.');
        setDepartments(demoDepartments);
        setEmployees(demoEmployees);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  useEffect(() => {
    if (selectedDepartmentId) {
      const firstEmployee = employees.find((employee) => employee.departmentId === selectedDepartmentId);
      if (firstEmployee) {
        setValue('employeeId', firstEmployee.id, { shouldValidate: true });
      }
    } else {
      setValue('employeeId', '');
    }
  }, [selectedDepartmentId, employees, setValue]);

  const availableEmployees = useMemo(
    () => employees.filter((employee) => employee.departmentId === selectedDepartmentId),
    [employees, selectedDepartmentId]
  );

  useEffect(() => {
    if (phoneValue) {
      const formatted = formatPhone(phoneValue);
      if (formatted !== phoneValue) {
        setValue('applicantPhone', formatted, { shouldValidate: true });
      }
    }
  }, [phoneValue, setValue]);

  useEffect(() => {
    if (passportValue) {
      const formatted = formatPassport(passportValue);
      if (formatted !== passportValue) {
        setValue('applicantPassport', formatted, { shouldValidate: true });
      }
    }
  }, [passportValue, setValue]);

  const onSubmit = async (values: PersonalApplicationForm) => {
    if (!isDateRangeValid(values.startDate, values.endDate)) {
      toast.error('Даты должны быть не раньше следующего дня и не позже 15 дней вперед');
      return;
    }

    try {
      const departmentId = Number.parseInt(values.departmentId, 10);
      const employeeId = Number.parseInt(values.employeeId, 10);
      if (Number.isNaN(departmentId) || Number.isNaN(employeeId)) {
        toast.error('Некорректно выбрано подразделение или ответственный сотрудник');
        return;
      }

      const applicantParticipant = {
        fullName: values.applicantName,
        birthDate: values.applicantBirthDate,
        passport: values.applicantPassport,
      };

      await submitApplication({
        type: 'personal',
        startDate: values.startDate,
        endDate: values.endDate,
        purpose: values.purpose,
        departmentId,
        employeeId,
        applicantName: values.applicantName,
        applicantPhone: values.applicantPhone,
        applicantEmail: user?.email || undefined,
        applicantBirthDate: values.applicantBirthDate,
        applicantPassport: values.applicantPassport,
        applicantUserId: user?.id,
        files: {
          photo: values.photo || undefined,
          passport: values.passportFile || undefined,
        },
        participants: [applicantParticipant],
      });
      toast.success('Заявка отправлена на рассмотрение');
      navigate('/dashboard');
    } catch (error: any) {
      toast.error(error?.response?.data?.message || 'Не удалось отправить заявку');
    }
  };

  return (
    <section className="flex flex-col gap-6">
      <header className="flex flex-col gap-2">
        <h1 className="text-2xl font-semibold text-primary">Личная заявка на посещение</h1>
        <p className="text-sm text-slate-600">
          Заполните данные гостя и загрузите необходимые документы. Заявка будет рассмотрена службой безопасности.
        </p>
      </header>

      {loading ? (
        <div className="py-20 text-center text-slate-500">Загрузка данных...</div>
      ) : (
        <form className="grid gap-6 rounded-xl border border-slate-200 bg-white p-8 shadow-sm" onSubmit={handleSubmit(onSubmit)}>
          <div className="grid gap-4 md:grid-cols-2">
            <FormInput label="Дата начала" type="date" error={formState.errors.startDate} {...register('startDate')} />
            <FormInput label="Дата окончания" type="date" error={formState.errors.endDate} {...register('endDate')} />
          </div>
          <FormInput
            label="Цель посещения"
            as="textarea"
            rows={3}
            error={formState.errors.purpose}
            placeholder="Например: деловая встреча, сопровождение подрядчика"
            {...register('purpose')}
          />
          <div className="grid gap-4 md:grid-cols-2">
            <label className="flex w-full flex-col gap-1 text-sm font-medium text-slate-700">
              <span>Подразделение</span>
              <select
                className="w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-primary focus:outline-none"
                {...register('departmentId')}
              >
                <option value="">Выберите подразделение</option>
                {departments.map((department) => (
                  <option key={department.id} value={department.id}>
                    {department.name}
                  </option>
                ))}
              </select>
              {formState.errors.departmentId && (
                <span className="text-xs font-medium text-red-500">{formState.errors.departmentId.message}</span>
              )}
            </label>
            <label className="flex w-full flex-col gap-1 text-sm font-medium text-slate-700">
              <span>Принимающий сотрудник</span>
              <select
                className="w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-sm focus:border-primary focus:outline-none"
                {...register('employeeId')}
              >
                <option value="">Выберите сотрудника</option>
                {availableEmployees.map((employee) => (
                  <option key={employee.id} value={employee.id}>
                    {employee.fullName}
                  </option>
                ))}
              </select>
              {formState.errors.employeeId && (
                <span className="text-xs font-medium text-red-500">{formState.errors.employeeId.message}</span>
              )}
            </label>
          </div>
          <div className="grid gap-4 md:grid-cols-2">
            <FormInput label="ФИО" error={formState.errors.applicantName} {...register('applicantName')} />
            <FormInput
              label="Телефон"
              placeholder="+7 (___) ___-__-__"
              error={formState.errors.applicantPhone}
              {...register('applicantPhone')}
            />
            <FormInput label="Дата рождения" type="date" error={formState.errors.applicantBirthDate} {...register('applicantBirthDate')} />
            <FormInput
              label="Паспорт"
              placeholder="1234 567890"
              error={formState.errors.applicantPassport}
              {...register('applicantPassport')}
            />
          </div>
          <div className="grid gap-6 md:grid-cols-2">
            <Controller
              name="photo"
              control={control}
              render={({ field }) => (
                <FileUpload
                  label="Фотография 3×4"
                  description="Формат JPG/PNG, размер до 4 МБ"
                  accept=".jpg,.jpeg,.png"
                  value={field.value}
                  onFileChange={field.onChange}
                  error={formState.errors.photo?.message}
                />
              )}
            />
            <Controller
              name="passportFile"
              control={control}
              render={({ field }) => (
                <FileUpload
                  label="Скан паспорта"
                  description="Формат PDF, размер до 4 МБ"
                  accept="application/pdf"
                  value={field.value}
                  onFileChange={field.onChange}
                  error={formState.errors.passportFile?.message}
                />
              )}
            />
          </div>
          <button
            type="submit"
            className="mt-4 rounded-md bg-primary px-6 py-2 text-white transition hover:bg-primary/90"
          >
            Отправить заявку
          </button>
        </form>
      )}
    </section>
  );
};
