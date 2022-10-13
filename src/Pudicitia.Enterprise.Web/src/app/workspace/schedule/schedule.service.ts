import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CalendarEvent } from 'shared/components/calendar/calendar-event.model';

@Injectable({
  providedIn: 'root',
})
export class ScheduleService {
  private urlEvents = 'api/schedule/events';

  constructor(
    private httpClient: HttpClient,
  ) { }

  getEvents(
    startedOn: Date,
    endedOn: Date,
  ): Observable<CalendarEvent[]> {
    const params = new HttpParams({
      fromObject: {
        startedOn: startedOn.toISOString(),
        endedOn: endedOn.toISOString(),
      },
    });
    return this.httpClient.get<CalendarEvent[]>(this.urlEvents, { params });
  }
}
