import { Pipe, PipeTransform } from '@angular/core';
import { SaveMode } from 'src/app/core/save-mode';

@Pipe({
  name: 'actionName',
})
export class ActionNamePipe implements PipeTransform {
  transform(value: SaveMode, args?: any): string {
    return SaveMode[value];
  }
}
