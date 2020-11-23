import { Pipe, PipeTransform } from '@angular/core';

import { SaveMode } from '../models/save-mode.enum';

@Pipe({
  name: 'actionName',
})
export class ActionNamePipe implements PipeTransform {
  transform(value: SaveMode | boolean, args?: any): string {
    if (typeof value === 'boolean') {
      return value ? 'Update' : 'Create';
    }
    return SaveMode[value];
  }
}
