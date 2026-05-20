import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { Users, User, LogOut } from "lucide-react";
import { useAuth } from "../context/AuthContext";
import { Logout as logoutApi } from "../api/auth";
import { UserRole } from "../types";
import { Avatar } from "./Avatar";

function Sidebar() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const canViewCoaches = user?.roles.includes(UserRole.Admin) || user?.roles.includes(UserRole.Manager);
    const canViewAthletes = user?.roles.includes(UserRole.Admin) || user?.roles.includes(UserRole.Manager) || user?.roles.includes(UserRole.Coach);

    async function handleLogout() {
        try {
            await logoutApi();
        } finally {
            logout();
            navigate('/login');
        }
    }

    return (
        <nav className="w-[240px] bg-sidebar min-h-screen flex flex-col px-3 py-5 shrink-0 border-r border-white/4">
            {/* Logo */}
            <div className="flex items-center gap-2.5 px-2 pb-7">
                <div className="w-[34px] h-[34px] rounded-[9px] bg-accent flex items-center justify-center font-head font-black text-[13px] text-white">NE</div>
                <span className="font-head font-bold text-[15px] text-white tracking-tight">noExcuses</span>
            </div>

            {/* Nav */}
            <div className="flex flex-col gap-1 flex-1">
                <span className="text-[10px] font-bold text-white/30 tracking-[.08em] uppercase px-2 mb-1.5">Gestão</span>

                {canViewCoaches && (
                    <NavLink to="/coaches" className={({ isActive }) =>
                        `flex items-center gap-2.5 px-3.5 py-2.5 rounded-lg text-sm font-head transition-colors
                          ${isActive ? 'bg-white/10 text-white font-semibold' : 'text-white/50 hover:bg-white/6 hover:text-white'}`
                    }>
                        {({ isActive }) => (
                            <>
                                <Users size={16} />
                                Treinadores
                                {isActive && <div className="ml-auto w-1.5 h-1.5 rounded-full bg-accent" />}
                            </>
                        )}
                    </NavLink>
                )}

                {canViewAthletes && (
                    <NavLink to="/athletes" className={({ isActive }) =>
                        `flex items-center gap-2.5 px-3.5 py-2.5 rounded-lg text-sm font-head transition-colors
                      ${isActive ? 'bg-white/10 text-white font-semibold' : 'text-white/50 hover:bg-white/6 hover:text-white'}`
                    }>
                        {({ isActive }) => (
                            <>
                                <User size={16} />
                                Atletas
                                {isActive && <div className="ml-auto w-1.5 h-1.5 rounded-full bg-accent" />}
                            </>
                        )}
                    </NavLink>
                )}
            </div>

            {/* User */}
            {user && (
                <div className="border-t border-white/7 pt-3.5 flex flex-col gap-1">
                    <div className="flex items-center gap-2.5 px-1.5 pb-2">
                        <Avatar name={user.name} />
                        <div className="overflow-hidden flex-1">
                            <div className="text-[13px] font-semibold text-white font-head truncate">{user.name}</div>
                            <div className="text-[11px] text-white/40 truncate">{user.email}</div>
                        </div>
                    </div>
                    <button
                        onClick={handleLogout}
                        className="flex items-center gap-2 w-full px-2 py-2 rounded-lg text-[13px] font-head text-white/45 hover:bg-white/ hover:text-white transition-colors cursor-pointer"
                    >
                        <LogOut size={14} />
                        Sair
                    </button>
                </div>
            )}
        </nav>
    );
}

export function AppShell() {
    return (
        <div className="flex min-h-screen">
            <Sidebar />
            <main className="flex-1 flex flex-col overflow-auto bg-bg">
                <Outlet />
            </main>
        </div>
    );
}