import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-input-chip',
  templateUrl: './input-chip.component.html',
  styleUrls: ['./input-chip.component.scss'],
})
export class InputChipComponent implements OnChanges {
  @Input() label = '';
  @Input() value?: string;
  @Output() valueChange = new EventEmitter<string>();

  isOpen = false;
  formGroup = this.formBuilder.group({
    value: '',
  })

  constructor(
    private formBuilder: FormBuilder,
  ) { }

  onRemoved = (): void => {
    this.formGroup.patchValue({ value: undefined });
    this.value = undefined;
    this.valueChange.emit(undefined);
  }

  ngOnChanges(changes: SimpleChanges): void {
    const value = changes['value'];
    if (value) {
      this.formGroup.patchValue({ value: value.currentValue });
    }
  }

  toggle(): void {
    this.isOpen = !this.isOpen;
    if (!this.isOpen) {
      this.formGroup.patchValue({ value: this.value });
    }
  }

  save(): void {
    const value = this.formGroup.getRawValue().value || undefined;
    this.value = value;
    this.valueChange.emit(value);
    this.isOpen = false;
  }
}
