import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { OperationResult } from '../models/api-response.model';

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
        roles: string[];
    };
}

const ACCESS_TOKEN_KEY = 'sporthub_admin_access_token';
const REFRESH_TOKEN_KEY = 'sporthub_admin_refresh_token';
const USER_KEY = 'sporthub_admin_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly apiUrl = '/api/admin/auth';

    private _currentUser = signal<LoginResponse['user'] | null>(this.loadUserFromStorage());
    currentUser = computed(() => this._currentUser());
    isAuthenticated = computed(() => !!this._currentUser());

    constructor(private http: HttpClient, private router: Router) { }

    login(request: LoginRequest): Observable<OperationResult<LoginResponse>> {
        return this.http
            .post<OperationResult<LoginResponse>>(`${this.apiUrl}/login`, request)
            .pipe(
                tap((res) => {
                    if (res.is_success) {
                        this.setSession(res.data);
                    }
                })
            );
    }

    logout(): void {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        this._currentUser.set(null);
        this.router.navigate(['/login']);
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
        this._currentUser.set(data.user);
    }

    private loadUserFromStorage(): LoginResponse['user'] | null {
        const raw = localStorage.getItem(USER_KEY);
        return raw ? JSON.parse(raw) : null;
    }
}