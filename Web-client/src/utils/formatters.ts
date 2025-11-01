export const formatPhone = (value: string) => {
  const digits = value.replace(/\D/g, '').replace(/^8/, '7');
  if (digits.length === 0) {
    return '';
  }

  const normalized = digits.startsWith('7') ? digits : `7${digits}`;
  const parts = ['+7'];
  if (normalized.length > 1) {
    parts.push(` (${normalized.slice(1, 4)}`);
  }
  if (normalized.length >= 4) {
    parts[parts.length - 1] += ')';
    parts.push(` ${normalized.slice(4, 7)}`);
  }
  if (normalized.length >= 7) {
    parts.push(`-${normalized.slice(7, 9)}`);
  }
  if (normalized.length >= 9) {
    parts.push(`-${normalized.slice(9, 11)}`);
  }
  return parts.join('').trim();
};

export const formatPassport = (value: string) => {
  const digits = value.replace(/\D/g, '');
  if (digits.length <= 4) {
    return digits;
  }
  return `${digits.slice(0, 4)} ${digits.slice(4, 10)}`.trim();
};

export const toISODate = (value: string) => {
  return new Date(value).toISOString();
};
