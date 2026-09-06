import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';
import { catchError, throwError, switchMap } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from '../../../../shared/service/alert.service';

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
            const isRefreshRequest = authRequest.url.includes('/auth/refresh-token');

            if (err.status === 401 && !isLoginRequest && !isRefreshRequest) {
                return authService.refreshToken().pipe(
                    switchMap((refreshRes) => {
                        if (!refreshRes.is_success) {
                            authService.logout();
                            router.navigate(['/login']);
                            return throwError(() => err);
                        }

                        const retryRequest = req.clone({
                            setHeaders: {
                                Authorization: `Bearer ${refreshRes.data.access_token}`
                            }
                        });

                        return next(retryRequest);
                    }),
                    catchError((refreshError) => {
                        authService.logout();
                        alertService.warning(
                            'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.'
                        );
                        router.navigate(['/login']);
                        return throwError(() => refreshError);
                    })
                );
            }

            return throwError(() => err);
        })
    );
};