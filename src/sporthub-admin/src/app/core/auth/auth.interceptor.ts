import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from '../common/alert/alert.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const alertService = inject(AlertService);
    const token = authService.getAccessToken();

    let authRequest = req;

    if (token) {
        authRequest = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
    }

    return next(authRequest).pipe(
        catchError((err: HttpErrorResponse) => {
            const isLoginRequest = authRequest.url.includes('/admin/auth/login');

            if (err.status === 401 && !isLoginRequest) {
                authService.logout();
                alertService.warning(
                    'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.'
                );
                router.navigate(['/login']);
            }

            return throwError(() => err);
        })
    );
};