import { ChangeEvent, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import classNames from 'classnames';

interface FileUploadProps {
  label: string;
  accept: string;
  description?: string;
  error?: string;
  onFileChange?: (file: File | null) => void;
  value?: File | null;
  maxSizeMb?: number;
}

const formatBytes = (bytes: number) => {
  if (bytes === 0) return '0 Б';
  const k = 1024;
  const sizes = ['Б', 'КБ', 'МБ'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return `${(bytes / Math.pow(k, i)).toFixed(1)} ${sizes[i]}`;
};

export const FileUpload: React.FC<FileUploadProps> = ({
  label,
  accept,
  description,
  error,
  value,
  onFileChange,
  maxSizeMb = 4,
}) => {
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  useEffect(() => {
    if (value && value.type.startsWith('image/')) {
      const url = URL.createObjectURL(value);
      setPreviewUrl(url);
      return () => {
        URL.revokeObjectURL(url);
      };
    }
    setPreviewUrl(null);
    return () => undefined;
  }, [value]);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) {
      setPreviewUrl(null);
      onFileChange?.(null);
      return;
    }

    if (file.size > maxSizeMb * 1024 * 1024) {
      onFileChange?.(null);
      setPreviewUrl(null);
      toast.error(`Файл превышает ${maxSizeMb} МБ`);
      return;
    }

    if (file.type.startsWith('image/')) {
      const url = URL.createObjectURL(file);
      setPreviewUrl(url);
    } else {
      setPreviewUrl(null);
    }

    onFileChange?.(file);
  };

  return (
    <div className="flex w-full flex-col gap-2 text-sm text-slate-700">
      <span className="font-medium">{label}</span>
      {description && <span className="text-xs font-normal text-slate-500">{description}</span>}
      <label
        className={classNames(
          'flex flex-col items-center justify-center gap-2 rounded-md border border-dashed border-slate-300 bg-white px-4 py-6 text-center transition hover:border-primary hover:bg-primary/5',
          error && 'border-red-500 text-red-500'
        )}
      >
        <span className="text-sm font-semibold text-primary">Загрузите файл</span>
        <span className="text-xs text-slate-500">
          Форматы: {accept}. Размер до {maxSizeMb} МБ
        </span>
        <input
          type="file"
          accept={accept}
          className="hidden"
          onChange={handleChange}
        />
      </label>
      {value && (
        <div className="rounded-md border border-slate-200 bg-white p-3 text-left">
          <p className="text-sm font-medium text-slate-700">{value.name}</p>
          <p className="text-xs text-slate-500">{formatBytes(value.size)}</p>
        </div>
      )}
      {previewUrl && (
        <div className="overflow-hidden rounded-md border border-slate-200">
          <img src={previewUrl} alt="Предпросмотр" className="h-40 w-full object-cover" />
        </div>
      )}
      {error && <span className="text-xs font-medium text-red-500">{error}</span>}
    </div>
  );
};
