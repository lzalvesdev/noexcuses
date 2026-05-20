import { createRoot } from 'react-dom/client'
import { AuthProvider } from './context/AuthContext'
import { RouterProvider } from 'react-router-dom'
import { router } from './router'
import './index.css'
import { Toaster } from 'sonner'

createRoot(document.getElementById('root')!).render(
  // <StrictMode>
  <AuthProvider>
    <RouterProvider router={router} />
    <Toaster richColors position="top-right" />
  </AuthProvider>
  // </StrictMode>,
)
