import { createContext, useContext, useEffect, useState } from "react";
import type { User } from "../types";
import { decodeToken, decodeTokenRaw } from "../utils/jwt";

interface AuthContextType {
    user: User | null;
    login: (user: User) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: ({ children: React.ReactNode })) {
    const [user, setUser] = useState<User | null>(() => {
        const token = localStorage.getItem('noExcuses_token');
        if (!token) return null;

        const decoded = decodeToken(token);
        if (decoded) return decoded;

        // Access token expirado — checa se refresh token ainda existe
        const refresh = localStorage.getItem('noExcuses_refresh');
        if (!refresh) {
            localStorage.removeItem('noExcuses_token');
            return null;
        }

        // Refresh token válido: carrega o usuário do token expirado para a UI funcionar.
        // O interceptor do client.ts vai renovar o token na primeira chamada à API.
        return decodeTokenRaw(token);
    });

    useEffect(() => {
        function handleForceLogout() {
            setUser(null);
        }
        window.addEventListener('auth:logout', handleForceLogout);
        return () => window.removeEventListener('auth:logout', handleForceLogout);
    }, []);

    function login(user: User) {
        setUser(user);
    }

    function logout() {
        setUser(null)
    }

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth deve ser usado dentro do AuthProvider')
    return ctx;
}