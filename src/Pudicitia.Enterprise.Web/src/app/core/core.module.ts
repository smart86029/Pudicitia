import { CommonModule } from '@angular/common';
import {
  ErrorHandler,
  ModuleWithProviders,
  NgModule,
  Optional,
  SkipSelf,
} from '@angular/core';

import { AppErrorHandler } from './app-error-handler';

@NgModule({
  declarations: [],
  imports: [CommonModule],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [{ provide: ErrorHandler, useClass: AppErrorHandler }],
    };
  }

  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only'
      );
    }
  }
}
