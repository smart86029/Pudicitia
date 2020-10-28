import { Pipe, PipeTransform } from '@angular/core';

import { SaveMode } from '../models/save-mode.enum';

@Pipe({
  name: 'actionName',
})
export class ActionNamePipe implements PipeTransform {
  transform(value: SaveMode, args?: any): string {
    return SaveMode[value];
  }
}
