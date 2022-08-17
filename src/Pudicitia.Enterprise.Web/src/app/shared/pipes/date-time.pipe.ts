import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateTime',
})
export class DateTimePipe implements PipeTransform {
  transform(date: Date | null | undefined, format?: 'date'): string | null {
    const datePipe = new DatePipe('en-US');
    switch (format) {
      case 'date':
        return datePipe.transform(date, 'yyyy-MM-dd');
      default:
        return datePipe.transform(date, 'yyyy-MM-dd HH:mm:ss');
    }
  }
}
