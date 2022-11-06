import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'workspace',
    loadChildren: () => import('./workspace/workspace.module').then(mod => mod.WorkspaceModule),
  },
  {
    path: 'management',
    loadChildren: () => import('./management/management.module').then(mod => mod.ManagementModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
