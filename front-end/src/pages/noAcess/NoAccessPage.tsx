import { useAuth } from "@/context/AuthContext";
import { ShieldOff } from "lucide-react";

export function NoAccessPage() {
    const { logout } = useAuth();

    return (
        <div className="min-h-screen flex flex-col items-center justify-center bg-bg gap-4">
            <ShieldOff size={40} className="text-muted" />
            <div className="text-center">
                <h1 className="font-head text-[20px] font-bold tracking-tight mb-1">Acesso não autorizado</h1>
                <p className="text-muted text-sm">Sua conta não tem permissão para acessar esta área.</p>
            </div>
            <button onClick={logout}
                className="bg-white mt-2 px-4 py-2 rounded-lg border border-border text-sm font-head font-semibold hover:bg-accent transition-colors cursor-pointer">
                Sair
            </button>
        </div>
    );
}