import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';

import { CalendarCell, DefaultCalendarCell } from '../calendar-cell';
import { HOURS_IN_DAY } from '../calendar.constant';
import { CalendarEvent } from '../calendar-event.model';

@Component({
  selector: 'app-calendar-day',
  templateUrl: './calendar-day.component.html',
  styleUrls: ['./calendar-day.component.scss'],
})
export class CalendarDayComponent<TDate> implements OnInit, OnChanges {
  dayOfWeekName = '';
  hours: number[] = [];
  cell: CalendarCell<TDate> = new DefaultCalendarCell<TDate>();
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<CalendarEvent[]>;

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initHours();
    this.date$
      .pipe(
        tap(date => this.createCell(date)),
        switchMap(date => this.getItems(date, date)),
        tap(events => this.setEvents(events)),
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.date$.next(<TDate>changes['date'].currentValue);
  }

  private initHours(): void {
    for (let i = 1; i <= HOURS_IN_DAY; i++) {
      this.hours.push(i);
    }
  }

  private createCell(date: TDate): void {
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('long');
    this.dayOfWeekName = dayOfWeekNames[this.dateAdapter.getDayOfWeek(date)];

    const today = this.dateAdapter.today()
    this.cell = {
      day: this.dateAdapter.getDate(date),
      date,
      isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
      isToday: this.dateAdapter.sameDate(date, today),
    };
  }

  private setEvents(events: CalendarEvent[]): void {
    if (this.cell) {
      this.cell.events = events;
    }
  }
}
