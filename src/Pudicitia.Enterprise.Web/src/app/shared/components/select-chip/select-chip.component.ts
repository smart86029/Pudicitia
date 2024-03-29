import { AfterViewInit, Component, ContentChildren, EventEmitter, Input, Output, QueryList } from '@angular/core';
import { MatOption } from '@angular/material/core';
import { MAT_SELECT_CONFIG } from '@angular/material/select';

@Component({
  selector: 'app-select-chip',
  templateUrl: './select-chip.component.html',
  styleUrls: ['./select-chip.component.scss'],
  providers: [
    {
      provide: MAT_SELECT_CONFIG,
      useValue: { overlayPanelClass: 'fit-content' },
    },
  ],
})
export class SelectChipComponent<TValue> implements AfterViewInit {
  @Input() label = '';
  @Input() value?: TValue;
  @Output() readonly valueChange = new EventEmitter<TValue | undefined>();

  isLoading = true;
  options: { value: TValue | undefined; viewValue: string }[] = [];

  @ContentChildren(MatOption) private matOptions!: QueryList<MatOption>;

  ngAfterViewInit(): void {
    this.options = this.matOptions.map(x => {
      return { value: x.value, viewValue: x.viewValue };
    });
    setTimeout(() => (this.isLoading = false));
  }

  onRemoved = (): void => {
    this.value = undefined;
    this.valueChange.emit(undefined);
  };

  onValueChange = (value?: TValue): void => {
    this.value = value;
    this.valueChange.emit(value);
  };
}
