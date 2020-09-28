import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ActionNamePipe } from './action-name.pipe';

@NgModule({
  declarations: [ActionNamePipe],
  imports: [CommonModule],
  exports: [ActionNamePipe],
})
export class PipesModule {}
