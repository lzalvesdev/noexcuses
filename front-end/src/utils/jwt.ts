import type { User } from '../types';

export function decodeToken(token: string): User | null {
    try {
        const base64 = token.split('.')[1]
            .replace(/-/g, '+')
            .replace(/_/g, '/');

        const payload = JSON.parse(decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
                .join('')
        ));

        if (payload.exp && Date.now() / 1000 > payload.exp) return null;

        const roleRaw = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? [];

        return {
            name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] ?? '',
            email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '',
            roles: ([] as string[]).concat(roleRaw),
        };
    } catch {
        return null;
    }
}