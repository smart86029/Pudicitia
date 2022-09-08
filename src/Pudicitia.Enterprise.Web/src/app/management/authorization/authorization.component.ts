import { Component } from '@angular/core';
import { Menu } from 'shared/models/menu.model';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.scss'],
})
export class AuthorizationComponent {
  menus: Menu[] = [
    { name: 'User', url: 'users' },
    { name: 'Role', url: 'roles' },
    { name: 'Permission', url: 'permissions' },
  ];
}
