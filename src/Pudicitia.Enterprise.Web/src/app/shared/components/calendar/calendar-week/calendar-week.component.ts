import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, map, Observable, switchMap, tap } from 'rxjs';

import { CalendarCell } from '../calendar-cell';
import { CalendarMode } from '../calendar-mode.enum';
import { DAYS_IN_WEEK, HOURS_IN_DAY } from '../calendar.constant';
import { CalendarEvent } from '../calendar-event.model';

@Component({
  selector: 'app-calendar-week',
  templateUrl: './calendar-week.component.html',
  styleUrls: ['./calendar-week.component.scss'],
})
export class CalendarWeekComponent<TDate> implements OnInit, OnChanges {
  dayOfWeekNames: string[] = [];
  hours: number[] = [];
  row: CalendarCell<TDate>[] = [];
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<CalendarEvent[]>;
  @Output() dateChange = new EventEmitter<TDate>();
  @Output() modeChange = new EventEmitter<CalendarMode>();

  constructor(
    private dateAdapter: DateAdapter<TDate>,
  ) { }

  ngOnInit(): void {
    this.initWeekdays();
    this.initHours();
    this.date$
      .pipe(
        map(date => this.getWeekRange(date)),
        tap(([firstDate]) => this.createCells(firstDate)),
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
    const dayOfWeekNames = this.dateAdapter.getDayOfWeekNames('long');
    this.dayOfWeekNames = dayOfWeekNames.slice(firstDayOfWeek).concat(dayOfWeekNames.slice(0, firstDayOfWeek));
  }

  private initHours(): void {
    for (let i = 1; i <= HOURS_IN_DAY; i++) {
      this.hours.push(i);
    }
  }

  private getWeekRange(date: TDate): [firstDate: TDate, lastDate: TDate] {
    const dayOfWeek = this.dateAdapter.getDayOfWeek(date);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_IN_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_IN_WEEK;
    const firstDate = this.dateAdapter.addCalendarDays(date, -firstWeekOffset)
    const lastDate = this.dateAdapter.addCalendarDays(firstDate, DAYS_IN_WEEK - 1);
    return [firstDate, lastDate];
  }

  private createCells(firstDate: TDate): void {
    this.row = [];
    const today = this.dateAdapter.today()

    for (let i = 0; i < DAYS_IN_WEEK; i++) {
      const date = this.dateAdapter.addCalendarDays(firstDate, i);
      this.row.push({
        day: this.dateAdapter.getDate(date),
        date,
        isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
        isToday: this.dateAdapter.sameDate(date, today),
      })
    }
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

    this.row.forEach(cell => {
      const key = this.dateAdapter.getDate(cell.date);
      if (dictionary.has(key)) {
        cell.events = dictionary.get(key)!;
      }
    })
  }
}
