import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PipesModule } from 'shared/pipes/pipes.module';

import { MaterialModule } from '../material/material.module';
import { CardLoadingComponent } from './card-loading/card-loading.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { DateRangeChipComponent } from './date-range-chip/date-range-chip.component';
import { SelectChipComponent } from './select-chip/select-chip.component';
import { TableComponent } from './table/table.component';
import { TreeSelectComponent } from './tree-select/tree-select.component';
import { TreeTableComponent } from './tree-table/tree-table.component';

@NgModule({
  declarations: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    DateRangeChipComponent,
    SelectChipComponent,
    TableComponent,
    TreeSelectComponent,
    TreeTableComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    PipesModule,
  ],
  exports: [
    CardLoadingComponent,
    ConfirmDialogComponent,
    DateRangeChipComponent,
    SelectChipComponent,
    TableComponent,
    TreeSelectComponent,
    TreeTableComponent,
  ],
})
export class ComponentsModule { }
