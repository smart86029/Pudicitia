import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PipesModule } from 'shared/pipes/pipes.module';

import { MaterialModule } from '../material/material.module';
import { BooleanSelectChipComponent } from './boolean-select-chip/boolean-select-chip.component';
import { CalendarMonthComponent } from './calendar/calendar-month/calendar-month.component';
import { CalendarWeekComponent } from './calendar/calendar-week/calendar-week.component';
import { CalendarComponent } from './calendar/calendar.component';
import { CardLoadingComponent } from './card-loading/card-loading.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { DateRangeChipComponent } from './date-range-chip/date-range-chip.component';
import { InputChipComponent } from './input-chip/input-chip.component';
import { SelectChipComponent } from './select-chip/select-chip.component';
import { TableComponent } from './table/table.component';
import { TreeSelectChipComponent } from './tree-select-chip/tree-select-chip.component';
import { TreeSelectComponent } from './tree-select/tree-select.component';
import { TreeTableComponent } from './tree-table/tree-table.component';

@NgModule({
  declarations: [
    BooleanSelectChipComponent,
    CalendarComponent,
    CalendarWeekComponent,
    CalendarMonthComponent,
    CardLoadingComponent,
    ConfirmDialogComponent,
    DateRangeChipComponent,
    InputChipComponent,
    SelectChipComponent,
    TableComponent,
    TreeSelectChipComponent,
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
    BooleanSelectChipComponent,
    CalendarComponent,
    CardLoadingComponent,
    ConfirmDialogComponent,
    DateRangeChipComponent,
    InputChipComponent,
    SelectChipComponent,
    TableComponent,
    TreeSelectChipComponent,
    TreeSelectComponent,
    TreeTableComponent,
  ],
})
export class ComponentsModule { }
