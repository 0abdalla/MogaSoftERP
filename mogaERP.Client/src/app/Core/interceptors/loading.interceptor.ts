import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, finalize, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { LoaderService } from '../services/loader.service';

export const loadingInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const loaderService = inject(LoaderService);
  const router = inject(Router);
  loaderService.show();
  const token = sessionStorage.getItem('token');
  const authReq = token
    ? req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      })
    : req;

  return next(authReq).pipe(
    catchError(error => {
      let message = 'حدث خطأ ما';

      if (error.status === 0) {
        message = 'مشكلة في الاتصال بالسيرفر';
      } else if (error.status === 401) {
        message = 'غير مسموح - يجب تسجيل الدخول';
        router.navigate(['/login']);
      } else if (error.error?.message) {
        message = error.error.message;
      }
      return throwError(() => ({ ...error, userMessage: message }));
    }),
    finalize(() => {
      loaderService.hide();
    })
  );
};
