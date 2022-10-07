import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, map, Observable, switchMap, tap } from 'rxjs';

import { CalendarCell } from '../calendar-cell';
import { CalendarMode } from '../calendar-mode.enum';
import { Event } from '../event.model';

const DAYS_PER_WEEK = 7;

@Component({
  selector: 'app-calendar-month',
  templateUrl: './calendar-month.component.html',
  styleUrls: ['./calendar-month.component.scss'],
})
export class CalendarMonthComponent<TDate> implements OnInit, OnChanges {
  weekdays: string[] = [];
  rows: CalendarCell<TDate>[][] = [];
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<Event[]>;
  @Output() dateChange = new EventEmitter<TDate>();
  @Output() modeChange = new EventEmitter<CalendarMode>();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initWeekdays();
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

  private initWeekdays(): void {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const weekdays = this.dateAdapter.getDayOfWeekNames('long');
    this.weekdays = weekdays.slice(firstDayOfWeek).concat(weekdays.slice(0, firstDayOfWeek));
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
    let row = [];
    const dateNames = this.dateAdapter.getDateNames();

    const dayOfWeek = this.dateAdapter.getDayOfWeek(firstDate);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_PER_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_PER_WEEK;

    for (let i = firstWeekOffset; i > 0; i--) {
      const date = this.dateAdapter.addCalendarDays(firstDate, -i);
      const value = this.dateAdapter.getDate(date);
      row.push({
        value: value,
        displayValue: dateNames[value - 1],
        isEnabled: false,
        isToday: this.dateAdapter.compareDate(date, today) === 0,
        date: date,
      })
    }

    const daysInMonth = this.dateAdapter.getNumDaysInMonth(firstDate);
    for (let i = 1, cell = firstWeekOffset; i <= daysInMonth; i++, cell++) {
      if (cell == DAYS_PER_WEEK) {
        this.rows.push(row);
        row = [];
        cell = 0;
      }
      const date = this.dateAdapter.createDate(year, month, i);
      row.push({
        value: i,
        displayValue: dateNames[i - 1],
        isEnabled: true,
        isToday: this.dateAdapter.compareDate(date, today) === 0,
        date: date,
      });
    }

    const remain = DAYS_PER_WEEK - row.length;
    for (let i = 1; i <= remain; i++) {
      const date = this.dateAdapter.addCalendarDays(lastDate, i);
      row.push({
        value: i,
        displayValue: dateNames[i - 1],
        isEnabled: false,
        isToday: this.dateAdapter.compareDate(date, today) === 0,
        date: date,
      });
    }

    this.rows.push(row);
  }

  private setEvents(events: Event[]): void {
    const dictionary = new Map<number, Event[]>();
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
