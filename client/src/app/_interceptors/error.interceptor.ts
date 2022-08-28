import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err) {
          switch (err.status) {
            case 400:
              if (err.error.errors) {
                const modalStateErorrs = [];
                for (const key in err.error.errors) {
                  if (err.error.errors[key]) {
                    modalStateErorrs.push(err.error.errors[key]);
                  }
                }
                throw modalStateErorrs.flat();
              } else if (typeof err.error === 'object') {
                this.toastr.error(err.statusText, err.status);
              } else {
                this.toastr.error(err.error, err.status);
              }
              break;
            case 401:
              this.toastr.error(err.statusText, err.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: err.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error(
                'An unexpected error occurred. Please try again later.'
              );
              console.log(err);
              break;
          }
        }
        return throwError(() => err);
      })
    );
  }
}
