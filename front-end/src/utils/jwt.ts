import type { User } from '../types';

function parsePayload(token: string): Record<string, unknown> | null {
    try {
        const base64 = token.split('.')[1]
            .replace(/-/g, '+')
            .replace(/_/g, '/');

        return JSON.parse(decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
                .join('')
        ));
    } catch {
        return null;
    }
}

function userFromPayload(payload: Record<string, unknown>): User {
    const roleRaw = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? [];
    return {
        name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] as string ?? '',
        email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] as string ?? '',
        roles: ([] as string[]).concat(roleRaw as string | string[]),
    };
}

export function decodeToken(token: string): User | null {
    const payload = parsePayload(token);
    if (!payload) return null;
    if (payload.exp && Date.now() / 1000 > (payload.exp as number)) return null;
    return userFromPayload(payload);
}

export function decodeTokenRaw(token: string): User | null {
    const payload = parsePayload(token);
    if (!payload) return null;
    return userFromPayload(payload);
}