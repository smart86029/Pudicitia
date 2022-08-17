import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';

@Component({
  selector: 'app-date-range-chip',
  templateUrl: './date-range-chip.component.html',
  styleUrls: ['./date-range-chip.component.scss'],
})
export class DateRangeChipComponent implements OnInit {
  hasValue = false;
  @Input() startedOn$ = new BehaviorSubject<Date | undefined>(undefined);
  @Input() endedOn$ = new BehaviorSubject<Date | undefined>(undefined);

  ngOnInit(): void {
    this.startedOn$
      .pipe(
        tap(startedOn => this.hasValue = startedOn !== undefined),
      )
      .subscribe();
  }

  clear(): void {
    this.startedOn$.next(undefined);
    this.endedOn$.next(undefined);
  }
}
