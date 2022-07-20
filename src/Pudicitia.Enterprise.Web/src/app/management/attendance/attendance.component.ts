import { Component } from '@angular/core';

import { Menu } from '../menu.model';

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
