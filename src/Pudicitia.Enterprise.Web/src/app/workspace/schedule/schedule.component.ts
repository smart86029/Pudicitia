import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, Observable, switchMap } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';
import { SaveMode } from 'shared/models/save-mode.enum';

import { EventDialogComponent } from './event-dialog/event-dialog.component';
import { ScheduleService } from './schedule.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent {
  interval$ = new BehaviorSubject<Interval>({ start: new Date(), end: new Date() });
  events$: Observable<CalendarEvent[]> = this.buildCalendarEvents();

  constructor(private dialog: MatDialog, private scheduleService: ScheduleService) {}

  private buildCalendarEvents(): Observable<CalendarEvent[]> {
    return this.interval$.pipe(switchMap(interval => this.scheduleService.getEvents(interval)));
  }

  onCellClick = (date: Date): void => {
    const dialogRef = this.dialog.open(EventDialogComponent, {
      data: { saveMode: SaveMode.Create, date },
    });
  };
}
