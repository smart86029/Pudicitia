import { Component, OnInit } from '@angular/core';
import { AttendanceService } from './attendance.service';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss'],
})
export class AttendanceComponent implements OnInit {

  constructor(private attendanceService: AttendanceService) { }

  ngOnInit(): void {
    console.log(1);
  }

}
