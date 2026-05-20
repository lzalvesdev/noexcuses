import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

interface Props {
    roles?: string[];
}

export function ProtectedRoute({ roles }: Props) {
    const { user } = useAuth();
    if (!user) return <Navigate to="/login" replace />;
    if (roles && !roles.some(r => user.roles.includes(r))) return <Navigate to="/" replace />;

    return <Outlet />;
}