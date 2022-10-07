import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';

import { CalendarCell, DefaultCalendarCell } from '../calendar-cell';
import { Event } from '../event.model';

@Component({
  selector: 'app-calendar-day',
  templateUrl: './calendar-day.component.html',
  styleUrls: ['./calendar-day.component.scss'],
})
export class CalendarDayComponent<TDate> implements OnInit, OnChanges {
  weekday = '';
  hours: number[] = [];
  cell: CalendarCell<TDate> = new DefaultCalendarCell<TDate>();
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<Event[]>;

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initHours();
    this.date$
      .pipe(
        tap(date => this.createCells(date)),
        switchMap(date => this.getItems(date, date)),
        tap(events => this.setEvents(events)),
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.date$.next(<TDate>changes['date'].currentValue);
  }

  private initHours(): void {
    for (let i = 1; i <= 23; i++) {
      this.hours.push(i);
    }
  }

  private createCells(date: TDate): void {
    const weekdays = this.dateAdapter.getDayOfWeekNames('long');
    this.weekday = weekdays[this.dateAdapter.getDayOfWeek(date)];

    const dateNames = this.dateAdapter.getDateNames();
    const today = this.dateAdapter.today()
    const value = this.dateAdapter.getDate(date);
    this.cell = {
      value: value,
      displayValue: dateNames[value - 1],
      isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
      isToday: this.dateAdapter.compareDate(date, today) === 0,
      date: date,
    };
  }

  private setEvents(events: Event[]): void {
    if (this.cell) {
      this.cell.events = events;
    }
  }
}
