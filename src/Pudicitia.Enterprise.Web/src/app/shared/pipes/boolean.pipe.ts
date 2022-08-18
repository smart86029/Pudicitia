import { Pipe, PipeTransform } from '@angular/core';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

@Pipe({
  name: 'boolean',
})
export class BooleanPipe implements PipeTransform {
  transform(value: boolean | null | undefined, format?: BooleanFormat): string {
    if (value === null || value === undefined) {
      return '';
    }
    switch (format) {
      case BooleanFormat.Enabled:
        return value ? 'Enabled' : 'Disabled';
      default:
        return value ? 'True' : 'False';
    }
  }
}
