import type { User } from "../types";
import { decodeToken } from "../utils/jwt";
import { client } from "./client";

interface LoginResponse {
    token: string;
    refreshToken: string;
    name: string,
    email: string
}

export async function Login(email: string, password: string): Promise<{ user: User }> {
    const { data } = await client.post<LoginResponse>('/Auth/login', { email, password });

    localStorage.setItem('noExcuses_token', data.token);
    localStorage.setItem('noExcuses_refresh', data.refreshToken);

    const decoded = decodeToken(data.token);
    if (!decoded) throw new Error('Token inválido recebido do servidor.');

    return {
        user: {
            name: data.name,
            email: data.email,
            roles: decoded.roles
        }
    };
}

export async function Logout(): Promise<void> {
    const refreshToken = localStorage.getItem('noExcuses_refresh')
    try {
        await client.post('/Auth/logout', { refreshToken })
    } finally {
        localStorage.removeItem('noExcuses_token');
        localStorage.removeItem('noExcuses_refresh');
    }
}

export async function Register(firstName: string, email: string, password: string): Promise<void> {
    await client.post('/Auth/register', { firstName, email, password });
}