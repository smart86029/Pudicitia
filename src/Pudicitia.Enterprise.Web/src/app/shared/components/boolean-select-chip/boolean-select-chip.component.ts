import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MAT_SELECT_CONFIG } from '@angular/material/select';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

@Component({
  selector: 'app-boolean-select-chip',
  templateUrl: './boolean-select-chip.component.html',
  styleUrls: ['./boolean-select-chip.component.scss'],
  providers: [
    {
      provide: MAT_SELECT_CONFIG,
      useValue: { overlayPanelClass: 'fit-content' },
    },
  ],
})
export class BooleanSelectChipComponent {
  @Input() label = '';
  @Input() onlyTrue = false;
  @Input() format?: BooleanFormat;
  @Input() value?: boolean;
  @Output() valueChange = new EventEmitter<boolean | undefined>();

  onRemoved = (): void => {
    this.value = undefined;
    this.valueChange.emit(undefined);
  };

  onValueChange = (value?: boolean): void => {
    this.value = value;
    this.valueChange.emit(value);
  };
}
