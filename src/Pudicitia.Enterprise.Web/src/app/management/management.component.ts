import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthService } from '../auth/auth.service';
import { Menu } from './menu';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.scss'],
})
export class ManagementComponent implements OnInit {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));
  nestedDataSource = new MatTreeNestedDataSource();
  nestedTreeControl = new NestedTreeControl<Menu>(this.getChildren);

  constructor(
    private breakpointObserver: BreakpointObserver,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const menus = this.getMenus();
    this.nestedDataSource.data = menus;
    this.nestedTreeControl.dataNodes = menus;
    this.nestedTreeControl.expandAll();
  }

  getChildren(menu: Menu): Observable<Menu[]> {
    return of(menu.children);
  }

  hasNestedChild(_: number, menu: Menu): boolean {
    return menu.children?.length > 0;
  }

  private getMenus(): Menu[] {
    const menus: Menu[] = [
      {
        name: 'Identity',
        icon: 'person',
        children: [
          { name: 'User', url: 'users' },
          { name: 'Role', url: 'roles' },
          { name: 'Permission', url: 'permissions' },
        ],
      },
    ];
    if (this.authService.hasPermission('hr')) {
      menus.push({
        name: 'Human Resources',
        icon: 'people',
        children: [{ name: 'Organization', url: 'hr/organization' }],
      });
    }
    return menus;
  }
}
