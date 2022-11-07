import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { formatISO } from 'date-fns';
import { Observable } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';

@Injectable({
  providedIn: 'root',
})
export class ScheduleService {
  private urlEvents = 'api/schedule/events';

  constructor(private httpClient: HttpClient) {}

  getEvents(interval: Interval): Observable<CalendarEvent[]> {
    const params = new HttpParams({
      fromObject: {
        startedOn: formatISO(interval.start),
        endedOn: formatISO(interval.end),
      },
    });
    return this.httpClient.get<CalendarEvent[]>(this.urlEvents, { params });
  }
}
