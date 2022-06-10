import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material/material.module';
import { CardLoadingComponent } from './card-loading/card-loading.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { TableComponent } from './table/table.component';

@NgModule({
  declarations: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    TableComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
  ],
  exports: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    TableComponent,
  ],
})
export class ComponentsModule { }
