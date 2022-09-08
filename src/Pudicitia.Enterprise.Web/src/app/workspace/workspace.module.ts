import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { WorkspaceRoutingModule } from './workspace-routing.module';
import { WorkspaceComponent } from './workspace.component';

@NgModule({
  declarations: [
    WorkspaceComponent,
  ],
  imports: [
    SharedModule,
    WorkspaceRoutingModule,
  ],
})
export class WorkspaceModule { }
