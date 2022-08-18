import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ActionNamePipe } from './action-name.pipe';
import { BooleanPipe } from './boolean.pipe';
import { DateTimePipe } from './date-time.pipe';
import { DictionaryPipe } from './dictionary.pipe';
import { EnumPipe } from './enum.pipe';

@NgModule({
  declarations: [
    ActionNamePipe,
    BooleanPipe,
    DateTimePipe,
    DictionaryPipe,
    EnumPipe,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    ActionNamePipe,
    BooleanPipe,
    DateTimePipe,
    DictionaryPipe,
    EnumPipe,
  ],
})
export class PipesModule { }
