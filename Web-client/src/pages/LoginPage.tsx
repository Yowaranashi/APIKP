import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FormInput } from '../components/FormInput';
import { useAuth } from '../hooks/useAuth';
import { Link, useNavigate } from 'react-router-dom';

interface LoginFormValues {
  email: string;
  password: string;
}

const schema = yup.object({
  email: yup.string().email('Введите корректный email').required('Email обязателен'),
  password: yup.string().required('Введите пароль'),
});

export const LoginPage = () => {
  const { handleSubmit, register, formState } = useForm<LoginFormValues>({
    resolver: yupResolver(schema),
  });
  const { login, loading } = useAuth();
  const navigate = useNavigate();

  const onSubmit = async (values: LoginFormValues) => {
    try {
      await login(values);
      navigate('/dashboard');
    } catch (error) {
      // Ошибка уже обработана в AuthProvider
    }
  };

  return (
    <section className="mx-auto mt-10 max-w-lg rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
      <h1 className="mb-6 text-2xl font-semibold text-primary">Вход для посетителей</h1>
      <p className="mb-8 text-sm text-slate-500">
        Введите email и пароль, указанные при регистрации личного кабинета посетителя. Если у вас нет аккаунта,{' '}
        <Link to="/register" className="text-primary hover:underline">
          зарегистрируйтесь
        </Link>
        .
        <br />Доступ сотрудников по внутренним учетным данным осуществляется через корпоративный портал.
      </p>
      <form className="flex flex-col gap-5" onSubmit={handleSubmit(onSubmit)}>
        <FormInput label="Email" type="email" error={formState.errors.email} {...register('email')} />
        <FormInput label="Пароль" type="password" error={formState.errors.password} {...register('password')} />
        <button
          type="submit"
          disabled={loading}
          className="mt-4 rounded-md bg-primary px-6 py-2 text-white transition hover:bg-primary/90 disabled:cursor-not-allowed disabled:bg-primary/60"
        >
          {loading ? 'Вход...' : 'Войти'}
        </button>
      </form>
    </section>
  );
};
