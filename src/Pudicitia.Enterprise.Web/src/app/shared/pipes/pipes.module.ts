import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ActionNamePipe } from './action-name.pipe';
import { BooleanPipe } from './boolean.pipe';
import { DateTimePipe } from './date-time.pipe';
import { DictionaryPipe } from './dictionary.pipe';
import { EnumPipe } from './enum.pipe';
import { IsUpdatePipe } from './is-update.pipe';

@NgModule({
  declarations: [
    ActionNamePipe,
    BooleanPipe,
    DateTimePipe,
    DictionaryPipe,
    EnumPipe,
    IsUpdatePipe,
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
    IsUpdatePipe,
  ],
})
export class PipesModule { }
