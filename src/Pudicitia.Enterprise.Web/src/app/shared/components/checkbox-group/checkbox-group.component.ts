/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-empty-function */
import {
  AfterContentInit,
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  forwardRef,
  HostBinding,
  OnDestroy,
  QueryList,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatCheckbox } from '@angular/material/checkbox';
import { combineLatest, map, startWith, Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-checkbox-group',
  templateUrl: './checkbox-group.component.html',
  styleUrls: ['./checkbox-group.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxGroupComponent),
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CheckboxGroupComponent implements AfterContentInit, OnDestroy, ControlValueAccessor {
  isDisabled = false;
  values: string[] = [];

  private destroyed$ = new Subject<void>();

  @HostBinding('class.checkbox-group') private checkboxGroup = true;
  @ContentChildren(MatCheckbox, { descendants: true }) private checkboxes!: QueryList<MatCheckbox>;

  onChange: (values: string[]) => void = () => {};
  onTouched: () => void = () => {};

  ngAfterContentInit(): void {
    if (this.values) {
      setTimeout(() => {
        this.setCheckBoxes();
        combineLatest(
          this.checkboxes.map(checkbox =>
            checkbox.change.pipe(
              map(x => x.source),
              startWith(checkbox),
            ),
          ),
        )
          .pipe(
            takeUntil(this.destroyed$),
            map(checkboxes => checkboxes.filter(x => x.checked).map(x => x.value)),
            tap(values => this.onChange(values)),
          )
          .subscribe();
      }, 0);
    }
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  writeValue(values: string[]): void {
    this.values = values;
    if (this.checkboxes) {
      this.setCheckBoxes();
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  private setCheckBoxes(): void {
    this.checkboxes.forEach(checkbox => {
      checkbox.checked = this.values.includes(checkbox.value);
    });
  }
}
