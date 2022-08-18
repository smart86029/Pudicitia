import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { BooleanFormat } from 'shared/models/boolean-format.enum';

@Component({
  selector: 'app-boolean-select-chip',
  templateUrl: './boolean-select-chip.component.html',
  styleUrls: ['./boolean-select-chip.component.scss'],
})
export class BooleanSelectChipComponent implements OnInit {
  hasValue = false;
  @Input() format?: BooleanFormat;
  @Input() value$ = new BehaviorSubject<boolean | undefined>(undefined);

  ngOnInit(): void {
    this.value$
      .pipe(
        tap(value => this.hasValue = value !== undefined),
      )
      .subscribe();
  }
}
