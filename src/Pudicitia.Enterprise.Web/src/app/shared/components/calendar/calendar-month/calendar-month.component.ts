import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, map, Observable, switchMap, tap } from 'rxjs';

import { CalendarCell } from '../calendar-cell';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK } from '../calendar.constant';
import { CalendarEvent } from '../calendar-event.model';

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
})
export class CalendarMonthComponent<TDate> implements OnInit, OnChanges {
  dayOfWeekNames: string[] = [];
  rows: CalendarCell<TDate>[][] = [];
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<CalendarEvent[]>;
  @Output() readonly dateChange = new EventEmitter<TDate>();
  @Output() readonly modeChange = new EventEmitter<CalendarMode>();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initNames();
    this.date$
      .pipe(
        map(date => this.getMonthRange(date)),
        tap(([firstDate, lastDate]) => this.createCells(firstDate, lastDate)),
        switchMap(([firstDate, lastDate]) => this.getItems(firstDate, lastDate)),
        tap(events => this.setEvents(events)),
      )
      .subscribe();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.date$.next(<TDate>changes['date'].currentValue);
  }

  selectDate(date: TDate) {
    this.dateChange.emit(date);
    this.modeChange.emit(CalendarMode.Day);
  }

  private initNames(): void {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('long');
    this.dayOfWeekNames = dayOfWeekNames.slice(firstDayOfWeek).concat(dayOfWeekNames.slice(0, firstDayOfWeek));
  }

  private getMonthRange(date: TDate): [firstDate: TDate, lastDate: TDate] {
    const year = this.dateAdapter.getYear(date);
    const month = this.dateAdapter.getMonth(date);
    const firstDate = this.dateAdapter.createDate(year, month, 1);
    const daysInMonth = this.dateAdapter.getNumDaysInMonth(firstDate);
    const lastDate = this.dateAdapter.createDate(year, month, daysInMonth);
    return [firstDate, lastDate];
  }

  private createCells(firstDate: TDate, lastDate: TDate): void {
    this.rows = [];
    const today = this.dateAdapter.today();
    const year = this.dateAdapter.getYear(firstDate);
    const month = this.dateAdapter.getMonth(firstDate);
    let row: CalendarCell<TDate>[] = [];

    const dayOfWeek = this.dateAdapter.getDayOfWeek(firstDate);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_IN_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_IN_WEEK;

    for (let i = firstWeekOffset; i > 0; i--) {
      const date = this.dateAdapter.addCalendarDays(firstDate, -i);
      const value = this.dateAdapter.getDate(date);
      row.push({
        day: value,
        date: date,
        isEnabled: false,
        isToday: this.dateAdapter.sameDate(date, today),
      })
    }

    const daysInMonth = this.dateAdapter.getNumDaysInMonth(firstDate);
    for (let day = 1, cell = firstWeekOffset; day <= daysInMonth; day++, cell++) {
      if (cell == DAYS_IN_WEEK) {
        this.rows.push(row);
        row = [];
        cell = 0;
      }
      const date = this.dateAdapter.createDate(year, month, day);
      row.push({
        day,
        date,
        isEnabled: true,
        isToday: this.dateAdapter.sameDate(date, today),
      });
    }

    const remain = DAYS_IN_WEEK - row.length;
    for (let day = 1; day <= remain; day++) {
      const date = this.dateAdapter.addCalendarDays(lastDate, day);
      row.push({
        day: day,
        date,
        isEnabled: false,
        isToday: this.dateAdapter.sameDate(date, today),
      });
    }

    this.rows.push(row);
  }

  private setEvents(events: CalendarEvent[]): void {
    const dictionary = new Map<number, CalendarEvent[]>();
    events.forEach(event => {
      const key = new Date(event.startedOn).getDate();
      if (!dictionary.has(key)) {
        dictionary.set(key, []);
      }
      dictionary.get(key)!.push(event);
    });
    this.rows.forEach(row => {
      row.forEach(cell => {
        const key = this.dateAdapter.getDate(cell.date);
        if (dictionary.has(key)) {
          cell.events = dictionary.get(key)!;
        }
      })
    })
  }
}
