import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';

@Component({
  selector: 'app-input-chip',
  templateUrl: './input-chip.component.html',
  styleUrls: ['./input-chip.component.scss'],
})
export class InputChipComponent implements OnInit {
  hasValue = false;
  isOpen = false;
  value?: string;
  @Input() value$ = new BehaviorSubject<string | undefined>(undefined);
  @Input() label?: string;

  ngOnInit(): void {
    this.value$
      .pipe(
        tap(value => {
          this.value = value;
          this.hasValue = value !== undefined;
        }),
      )
      .subscribe();
  }

  toggle(): void {
    this.value = this.isOpen ? undefined : this.value$.getValue();
    this.isOpen = !this.isOpen;
  }

  save(): void {
    this.isOpen = false;
    this.value$.next(this.value);
  }
}
