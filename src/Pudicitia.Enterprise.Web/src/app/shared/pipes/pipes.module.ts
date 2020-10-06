import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ActionNamePipe } from './action-name.pipe';
import { DictionaryPipe } from './dictionary.pipe';

@NgModule({
  declarations: [ActionNamePipe, DictionaryPipe],
  imports: [CommonModule],
  exports: [ActionNamePipe, DictionaryPipe],
})
export class PipesModule {}
