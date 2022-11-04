import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hasValue',
})
export class HasValuePipe implements PipeTransform {
  transform(value: unknown): unknown {
    return value !== undefined;
  }
}
