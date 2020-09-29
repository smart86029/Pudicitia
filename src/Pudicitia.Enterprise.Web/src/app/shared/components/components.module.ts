import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material/material.module';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { ThemePickerComponent } from './theme-picker/theme-picker.component';

@NgModule({
  declarations: [ConfirmDialogComponent, ThemePickerComponent],
  imports: [CommonModule, MaterialModule],
  exports: [ConfirmDialogComponent, ThemePickerComponent],
})
export class ComponentsModule {}
