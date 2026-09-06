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