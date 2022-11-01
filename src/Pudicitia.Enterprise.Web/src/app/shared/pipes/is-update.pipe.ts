import { Pipe, PipeTransform } from '@angular/core';
import { SaveMode } from 'shared/models/save-mode.enum';

@Pipe({
  name: 'isUpdate',
})
export class IsUpdatePipe implements PipeTransform {
  transform(saveMode: SaveMode): boolean {
    return saveMode === SaveMode.Update;
  }
}
