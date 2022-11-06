import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ComponentsModule } from './components/components.module';
import { MaterialModule } from './material/material.module';
import { PipesModule } from './pipes/pipes.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    CommonModule,
    ComponentsModule,
    FormsModule,
    MaterialModule,
    PipesModule,
    ReactiveFormsModule,
  ],
})
export class SharedModule {}
