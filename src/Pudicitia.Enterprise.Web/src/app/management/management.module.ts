import { NgModule } from '@angular/core';
import { SharedModule } from 'shared/shared.module';

import { ManagementRoutingModule } from './management-routing.module';
import { ManagementComponent } from './management.component';

@NgModule({
  declarations: [
    ManagementComponent,
  ],
  imports: [
    SharedModule,
    ManagementRoutingModule,
  ],
})
export class ManagementModule { }
