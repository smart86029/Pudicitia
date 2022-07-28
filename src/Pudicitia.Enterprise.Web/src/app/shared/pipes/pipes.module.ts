import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ActionNamePipe } from './action-name.pipe';
import { DictionaryPipe } from './dictionary.pipe';
import { EnumPipe } from './enum.pipe';

@NgModule({
  declarations: [
    ActionNamePipe,
    DictionaryPipe,
    EnumPipe,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    ActionNamePipe,
    DictionaryPipe,
    EnumPipe,
  ],
})
export class PipesModule { }
