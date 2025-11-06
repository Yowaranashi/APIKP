import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FormInput } from '../components/FormInput';
import { useAuth } from '../hooks/useAuth';
import { Link, useNavigate } from 'react-router-dom';
import { isValidPassword } from '../utils/validators';

interface RegisterFormValues {
  email: string;
  fullName: string;
  password: string;
  confirmPassword: string;
}

const DEFAULT_ROLE_ID = 2; // роль "Пользователь" по умолчанию

const schema = yup.object({
  email: yup.string().trim().email('Введите корректный email').required('Email обязателен'),
  fullName: yup.string().trim().required('Укажите ФИО'),
  password: yup
    .string()
    .required('Введите пароль')
    .test('password-strong', 'Пароль должен содержать минимум 8 символов, цифру и спецсимвол', isValidPassword),
  confirmPassword: yup
    .string()
    .required('Повторите пароль')
    .oneOf([yup.ref('password')], 'Пароли должны совпадать'),
});

export const RegisterPage = () => {
  const { handleSubmit, register, formState } = useForm<RegisterFormValues>({
    resolver: yupResolver(schema),
  });
  const { register: registerUser, loading } = useAuth();
  const navigate = useNavigate();

  const onSubmit = async (values: RegisterFormValues) => {
    try {
      await registerUser({
        email: values.email,
        fullName: values.fullName.trim(),
        password: values.password,
        roleId: DEFAULT_ROLE_ID,
      });

      navigate('/dashboard');
    } catch (error) {
      // Ошибка уже показана пользователю
    }
  };

  return (
    <section className="mx-auto mt-10 max-w-lg rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
      <h1 className="mb-6 text-2xl font-semibold text-primary">Регистрация</h1>
      <p className="mb-8 text-sm text-slate-500">
        Создайте учетную запись для подачи заявок на посещение предприятия. Уже есть аккаунт?{' '}
        <Link to="/login" className="text-primary hover:underline">
          Войдите
        </Link>
        .
      </p>
      <form className="flex flex-col gap-5" onSubmit={handleSubmit(onSubmit)}>
        <FormInput label="Email" type="email" error={formState.errors.email} {...register('email')} />
        <FormInput label="ФИО" error={formState.errors.fullName} {...register('fullName')} />
        <FormInput label="Пароль" type="password" error={formState.errors.password} {...register('password')} />
        <FormInput
          label="Подтверждение пароля"
          type="password"
          error={formState.errors.confirmPassword}
          {...register('confirmPassword')}
        />
        <button
          type="submit"
          disabled={loading}
          className="mt-4 rounded-md bg-primary px-6 py-2 text-white transition hover:bg-primary/90 disabled:cursor-not-allowed disabled:bg-primary/60"
        >
          {loading ? 'Регистрация...' : 'Зарегистрироваться'}
        </button>
      </form>
    </section>
  );
};
