import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, finalize } from 'rxjs';
import { OperationResult } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    access_token: string;
    access_token_expires_at: string;
    refresh_token: string;
    user: {
        id: number;
        email: string;
        name: string;
        phone: string;
        roles: string[];
        avatar_url: string;
        status: boolean;
    };
}

const ACCESS_TOKEN_KEY = 'sporthub_admin_access_token';
const REFRESH_TOKEN_KEY = 'sporthub_admin_refresh_token';
const USER_KEY = 'sporthub_admin_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly apiUrl = `${environment.apiUrl}/sporthub`;

    private _currentUser = signal<LoginResponse['user'] | null>(this.loadUserFromStorage());
    currentUser = computed(() => this._currentUser());
    isAuthenticated = computed(() => !!this._currentUser());

    constructor(private http: HttpClient, private router: Router) { }

    login(request: LoginRequest): Observable<OperationResult<LoginResponse>> {
        return this.http
            .post<OperationResult<LoginResponse>>(`${this.apiUrl}/admin/auth/login`, request)
            .pipe(
                tap((res) => {
                    if (res.is_success) {
                        this.setSession(res.data);
                    }
                })
            );
    }

    logout(): Observable<OperationResult<boolean>> {
        return this.http
            .post<OperationResult<boolean>>(
                `${this.apiUrl}/auth/logout`,
                { refresh_token: this.getRefreshToken() }
            )
            .pipe(
                finalize(() => this.clearSession())
            );
    }

    refreshToken(): Observable<OperationResult<LoginResponse>> {
        return this.http
            .post<OperationResult<LoginResponse>>(
                `${this.apiUrl}/auth/refresh-token`,
                { refresh_token: this.getRefreshToken() }
            )
            .pipe(
                tap((res) => {
                    if (res.is_success) {
                        this.setSession(res.data);
                    }
                })
            );
    }

    getAccessToken(): string | null {
        return localStorage.getItem(ACCESS_TOKEN_KEY);
    }

    getRefreshToken(): string | null {
        return localStorage.getItem(REFRESH_TOKEN_KEY);
    }

    hasRole(role: string): boolean {
        return this._currentUser()?.roles.includes(role) ?? false;
    }

    private setSession(data: LoginResponse): void {
        localStorage.setItem(ACCESS_TOKEN_KEY, data.access_token);
        localStorage.setItem(REFRESH_TOKEN_KEY, data.refresh_token);
        localStorage.setItem(USER_KEY, JSON.stringify(data.user));
        console.log(this._currentUser)
        this._currentUser.set(data.user);
    }

    private loadUserFromStorage(): LoginResponse['user'] | null {
        const raw = localStorage.getItem(USER_KEY);
        return raw ? JSON.parse(raw) : null;
    }

    private clearSession(): void {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        this._currentUser.set(null);
    }
}