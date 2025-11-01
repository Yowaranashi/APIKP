import dayjs from 'dayjs';

export const isValidPassword = (value: string): boolean => {
  const regex = /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/;
  return regex.test(value);
};

export const isValidPhone = (value: string): boolean => {
  const regex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
  return regex.test(value);
};

export const isValidPassport = (value: string): boolean => {
  const regex = /^\d{4} \d{6}$/;
  return regex.test(value);
};

export const isAdult = (birthDate: string, minAge = 16) => {
  if (!birthDate) return false;
  return dayjs().diff(dayjs(birthDate), 'year') >= minAge;
};

export const isDateRangeValid = (startDate: string, endDate: string) => {
  const start = dayjs(startDate);
  const end = dayjs(endDate);
  const tomorrow = dayjs().add(1, 'day').startOf('day');
  const maxDate = dayjs().add(15, 'day').endOf('day');

  if (!start.isValid() || !end.isValid()) {
    return false;
  }

  return start.isAfter(tomorrow.subtract(1, 'minute')) && end.isAfter(start.subtract(1, 'minute')) && end.isBefore(maxDate.add(1, 'minute'));
};
