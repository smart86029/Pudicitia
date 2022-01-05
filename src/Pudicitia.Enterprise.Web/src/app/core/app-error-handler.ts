import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable()
export class AppErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) { }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  handleError(error: any): void {
    const ngZone = this.injector.get(NgZone);
    const router = this.injector.get(Router);
    const snackBar = this.injector.get(MatSnackBar);

    console.error(error);
    if (error instanceof HttpErrorResponse) {
      if (error.status === 401) {
        ngZone
          .run(() => router.navigate(['/auth/signin'], { queryParams: { returnUrl: router.url } }))
          .then();
      }
      snackBar.open(error.message);
    } else {
      snackBar.open(error);
    }
  }
}
