import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-date-range-chip',
  templateUrl: './date-range-chip.component.html',
  styleUrls: ['./date-range-chip.component.scss'],
})
export class DateRangeChipComponent {
  @Input() startedOn?: Date;
  @Input() endedOn?: Date;
  @Output() startedOnChange = new EventEmitter<Date | undefined>();
  @Output() endedOnChange = new EventEmitter<Date | undefined>();

  onStartedOnChange = (startedOn?: Date): void => {
    this.startedOn = startedOn;
    this.startedOnChange.emit(startedOn);
  }

  onEndedOnChange = (endedOn?: Date): void => {
    this.endedOn = endedOn;
    this.endedOnChange.emit(endedOn);
  }

  onRemoved = (): void => {
    this.startedOn = undefined;
    this.endedOn = undefined;
    this.startedOnChange.emit(undefined);
    this.endedOnChange.emit(undefined);
  }
}
