import { toast } from 'sonner';
import { CheckCircle2, XCircle, AlertTriangle } from 'lucide-react';

const config = {
    success: { color: '#b6f8c4', Icon: CheckCircle2 },
    error: { color: '#fca5a5', Icon: XCircle },
    alert: { color: '#fef08a', Icon: AlertTriangle },
} as const;

type ToastType = keyof typeof config;

function ToastContent({ message, type }: { message: string; type: ToastType }) {
    const { color, Icon } = config[type];
    return (
        <div className="flex items-center gap-3 bg-[#0f1117] border border-white/10 rounded-xl px-4 py-3 shadow-2xl
  min-w-[280px] max-w-[360px]">
            <Icon size={16} style={{ color }} className="shrink-0" />
            <span className="text-sm font-head text-white leading-snug">{message}</span>
        </div>
    );
}

export function showSuccess(message: string, duration = 4000) {
    toast.custom(() => <ToastContent message={message} type="success" />, { duration: duration });
}

export function showError(message: string, duration = 4000) {
    toast.custom(() => <ToastContent message={message} type="error" />, { duration: duration });
}

export function showAlert(message: string, duration = 4000) {
    toast.custom(() => <ToastContent message={message} type="alert" />, { duration: duration });
}