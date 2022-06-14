import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MaterialModule } from '../material/material.module';
import { CardLoadingComponent } from './card-loading/card-loading.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { TableComponent } from './table/table.component';
import { TreeTableComponent } from './tree-table/tree-table.component';

@NgModule({
  declarations: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    TableComponent,
    TreeTableComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
  ],
  exports: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    TableComponent,
    TreeTableComponent,
  ],
})
export class ComponentsModule { }
