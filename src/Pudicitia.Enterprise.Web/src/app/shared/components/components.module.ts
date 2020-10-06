import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material/material.module';
import { CardLoadingComponent } from './card-loading/card-loading.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { ThemePickerComponent } from './theme-picker/theme-picker.component';

@NgModule({
  declarations: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    ThemePickerComponent,
  ],
  imports: [CommonModule, MaterialModule],
  exports: [CardLoadingComponent, ConfirmDialogComponent, ThemePickerComponent],
})
export class ComponentsModule {}
