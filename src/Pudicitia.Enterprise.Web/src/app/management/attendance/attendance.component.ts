import { Component } from '@angular/core';
import { Menu } from 'shared/models/menu.model';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss'],
})
export class AttendanceComponent {
  menus: Menu[] = [
    { name: 'Leave', url: 'leaves' },
  ];
}
