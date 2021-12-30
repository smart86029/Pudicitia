import { Pipe, PipeTransform } from '@angular/core';

import { SaveMode } from '../models/save-mode.enum';

@Pipe({
  name: 'actionName',
})
export class ActionNamePipe implements PipeTransform {
  transform(value: boolean | SaveMode): string {
    if (typeof value === 'boolean') {
      return value ? 'Update' : 'Create';
    }
    return SaveMode[value];
  }
}
