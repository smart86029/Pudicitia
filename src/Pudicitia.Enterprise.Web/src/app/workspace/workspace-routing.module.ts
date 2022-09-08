import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/auth.guard';
import { WorkspaceComponent } from './workspace.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    component: WorkspaceComponent,
    children: [

    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkspaceRoutingModule { }
