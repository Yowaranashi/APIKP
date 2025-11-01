import { InputHTMLAttributes, TextareaHTMLAttributes } from 'react';
import { FieldError } from 'react-hook-form';
import classNames from 'classnames';

type Props = {
  label: string;
  error?: FieldError;
  description?: string;
  as?: 'input' | 'textarea';
} & InputHTMLAttributes<HTMLInputElement> &
  TextareaHTMLAttributes<HTMLTextAreaElement>;

export const FormInput: React.FC<Props> = ({
  label,
  error,
  description,
  as = 'input',
  className,
  ...props
}) => {
  const Component = as;
  return (
    <label className="flex w-full flex-col gap-1 text-sm font-medium text-slate-700">
      <span>{label}</span>
      {description && <span className="text-xs font-normal text-slate-500">{description}</span>}
      <Component
        className={classNames(
          'w-full rounded-md border border-slate-200 bg-white px-3 py-2 text-sm transition focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary',
          error && 'border-red-500 focus:border-red-500 focus:ring-red-500',
          className
        )}
        {...props}
      />
      {error && <span className="text-xs font-medium text-red-500">{error.message}</span>}
    </label>
  );
};
