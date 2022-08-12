import { Component } from '@angular/core';

import { Menu } from '../menu.model';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
})
export class OrganizationComponent {
  menus: Menu[] = [
    { name: 'Department', url: 'departments' },
    { name: 'Employee', url: 'employees' },
    { name: 'Job', url: 'jobs' },
  ];
}
