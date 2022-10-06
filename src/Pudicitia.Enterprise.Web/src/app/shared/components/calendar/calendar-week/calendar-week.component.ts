import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { BehaviorSubject, map, Observable, switchMap, tap } from 'rxjs';
import { CalendarCell } from '../calendar-cell';
import { Event } from '../event.model';

const DAYS_PER_WEEK = 7;

@Component({
  selector: 'app-calendar-week',
  templateUrl: './calendar-week.component.html',
  styleUrls: ['./calendar-week.component.scss'],
})
export class CalendarWeekComponent<TDate> implements OnInit, OnChanges {
  weekdays: string[] = [];
  hours: number[] = [];
  row: CalendarCell<TDate>[] = [];
  date$: BehaviorSubject<TDate> = new BehaviorSubject<TDate>(this.dateAdapter.today());

  @Input() date!: TDate;
  @Input() getItems!: (startedOn: TDate, endedOn: TDate) => Observable<Event[]>;
  @Output() dateChange = new EventEmitter<TDate>();

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
    const date = changes['date'];
    if (!date.firstChange) {
      this.date$.next(<TDate>date.currentValue);
    }
  }

  private initWeekdays(): void {
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const weekdays = this.dateAdapter.getDayOfWeekNames('long');
    this.weekdays = weekdays.slice(firstDayOfWeek).concat(weekdays.slice(0, firstDayOfWeek));
  }

  private initHours(): void {
    for (let i = 1; i <= 23; i++) {
      this.hours.push(i);
    }
  }

  private getWeekRange(date: TDate): [firstDate: TDate, lastDate: TDate] {
    const dayOfWeek = this.dateAdapter.getDayOfWeek(date);
    const firstDayOfWeek = this.dateAdapter.getFirstDayOfWeek();
    const firstWeekOffset = (DAYS_PER_WEEK + dayOfWeek - firstDayOfWeek) % DAYS_PER_WEEK;
    const firstDate = this.dateAdapter.addCalendarDays(date, -firstWeekOffset)
    const lastDate = this.dateAdapter.addCalendarDays(firstDate, DAYS_PER_WEEK - 1);
    return [firstDate, lastDate];
  }

  private createCells(firstDate: TDate): void {
    this.row = [];
    const dateNames = this.dateAdapter.getDateNames();
    const today = this.dateAdapter.today()

    for (let i = 0; i < DAYS_PER_WEEK; i++) {
      const date = this.dateAdapter.addCalendarDays(firstDate, i);
      const value = this.dateAdapter.getDate(date);
      this.row.push({
        value: value,
        displayValue: dateNames[value - 1],
        isEnabled: this.dateAdapter.compareDate(date, today) >= 0,
        date: date,
      })
    }
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

    this.row.forEach(cell => {
      const key = this.dateAdapter.getDate(cell.date);
      if (dictionary.has(key)) {
        cell.events = dictionary.get(key)!;
      }
    })
  }
}
