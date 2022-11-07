import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { toDate } from 'date-fns';

@Component({
  selector: 'app-date-range-chip',
  templateUrl: './date-range-chip.component.html',
  styleUrls: ['./date-range-chip.component.scss'],
})
export class DateRangeChipComponent implements OnChanges {
  @Input() interval?: Interval;
  @Output() readonly intervalChange = new EventEmitter<Interval | undefined>();

  start?: Date;
  end?: Date;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['interval']?.currentValue) {
      const { start, end } = changes['interval'].currentValue as Interval;
      this.start = toDate(start);
      this.end = toDate(end);
    }
  }

  onStartChange = (start?: Date): void => {
    this.start = start;
  };

  onEndChange = (end?: Date): void => {
    this.end = end;
    if (end) {
      this.intervalChange.emit({ start: this.start!, end });
    } else if (!this.start) {
      this.intervalChange.emit(undefined);
    }
  };

  onRemoved = (): void => {
    this.start = undefined;
    this.interval = undefined;
    this.intervalChange.emit(undefined);
  };
}
