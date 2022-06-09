import { Component } from '@angular/core';

import { Menu } from '../menu.model';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.scss'],
})
export class AuthorizationComponent {
  userCount = 0;
  roleCount = 0;
  permissionCount = 0;
  menus: Menu[] = [
    { name: 'User', url: 'users' },
    { name: 'Role', url: 'roles' },
    { name: 'Permission', url: 'permissions' },
  ];
}
