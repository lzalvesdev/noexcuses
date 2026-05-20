import { createBrowserRouter, Navigate } from "react-router-dom";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { useAuth } from "./context/AuthContext";
import { LoginPage } from "./pages/LoginPage";
import { GuestRoute } from "./components/GuestRoute";
import { AppShell } from "./components/AppShell";
import { UserRole } from "./types";
import { CoachPage } from "./pages/coaches/CoachPage";
import { AthletePage } from "./pages/athletes/AthletePage";
import { NoAccessPage } from "./pages/noAcess/NoAccessPage";

function HomePage() {
    const { user } = useAuth();
    if (user?.roles.includes(UserRole.Admin) || user?.roles.includes(UserRole.Manager)) return <Navigate to="/coaches" replace />;
    if (user?.roles.includes(UserRole.Coach)) return <Navigate to="/athletes" replace />;
    return <Navigate to="/no-access" replace />;
}

export const router = createBrowserRouter([
    {
        element: <GuestRoute />,
        children: [{ path: '/login', element: <LoginPage /> }]
    },
    {
        element: <ProtectedRoute />,
        children: [
            {
                element: <AppShell />,
                children: [
                    { path: '/', element: <HomePage /> },
                    {
                        element: <ProtectedRoute roles={[UserRole.Admin, UserRole.Manager]} />,
                        children: [
                            { path: '/coaches', element: <CoachPage /> }
                        ]
                    },
                    {
                        element: <ProtectedRoute roles={[UserRole.Admin, UserRole.Manager, UserRole.Coach]} />,
                        children: [
                            { path: '/athletes', element: <AthletePage /> },
                        ],
                    },
                    { path: '/no-access', element: <NoAccessPage /> },
                ]
            }
        ],
    },
    { path: '*', element: <Navigate to="/" replace /> },
]);