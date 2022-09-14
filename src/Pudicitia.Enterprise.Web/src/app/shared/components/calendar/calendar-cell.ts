export interface CalendarCell<TDate> {
  value: number;
  displayValue: string;
  isEnabled: boolean;
  date: TDate,
}
