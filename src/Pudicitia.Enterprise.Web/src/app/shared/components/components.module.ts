import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material/material.module';
import { ThemePickerComponent } from './theme-picker/theme-picker.component';

@NgModule({
  declarations: [ThemePickerComponent],
  imports: [CommonModule, MaterialModule],
  exports: [ThemePickerComponent],
})
export class ComponentsModule {}
