import { AfterViewInit, Component, ContentChildren, Input, OnInit, QueryList } from '@angular/core';
import { MatOption } from '@angular/material/core';
import { BehaviorSubject, tap } from 'rxjs';

@Component({
  selector: 'app-select-chip',
  templateUrl: './select-chip.component.html',
  styleUrls: ['./select-chip.component.scss'],
})
export class SelectChipComponent<TValue> implements OnInit, AfterViewInit {
  isLoading = true;
  hasValue = false;
  options: { value: TValue | undefined, viewValue: string }[] = [];
  @Input() label = '';
  @Input() value$ = new BehaviorSubject<TValue | undefined>(undefined);

  @ContentChildren(MatOption) private matOptions!: QueryList<MatOption>;

  ngOnInit(): void {
    this.value$
      .pipe(
        tap(value => this.hasValue = value !== undefined),
      )
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.options = this.matOptions.map(x => {
      return { value: x.value, viewValue: x.viewValue };
    });
    setTimeout(() => this.isLoading = false);
  }
}
