import { registerCoach } from "@/api/coach";
import type { Coach } from "@/types";
import { showError, showSuccess } from "@/utils/toast";
import { X } from "lucide-react";
import { useState } from "react";

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export function CreateCoachModal({ onClose, onDone }: { onClose: () => void; onDone: (coach: Coach) => void }) {
    const [firstName, setFirstName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [busy, setBusy] = useState(false);

    const canSubmit = firstName.trim().length > 0 && emailRegex.test(email) && password.length >= 1;

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setBusy(true);
        try {
            const coach = await registerCoach(firstName, email, password);
            showSuccess('Coach criado com sucesso!');
            onDone(coach);
            onClose();
        } catch {
            showError('Erro ao criar coach, tente novamente.');
        } finally {
            setBusy(false);
        }
    }

    return (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-xs z-50 flex items-center justify-center">
            <div className="bg-white rounded-2xl p-7 w-full max-w-[420px] shadow-[0_20px_60px_rgba(0,0,0,.2)]"
                onClick={e => e.stopPropagation()}>
                <div className="flex items-center justify-between mb-1.5">
                    <h2 className="font-head text-[18px] font-bold">Novo Coach</h2>
                    <button onClick={onClose}
                        className="text-muted hover:text-sidebar p-1.5 rounded-lg hover:bg-bg transition-colors cursor-pointer">
                        <X size={18} />
                    </button>
                </div>
                <p className="text-muted text-[13px] leading-relaxed mb-6">
                    Uma conta de usuário será criada e promovida a coach.
                </p>

                <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                    <div className="flex flex-col gap-1.5">
                        <label className="font-head text-sm font-semibold">Nome completo</label>
                        <input value={firstName} onChange={e => setFirstName(e.target.value)}
                            placeholder="Digite um nome"
                            autoComplete="off"
                            className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
                    </div>
                    <div className="flex flex-col gap-1.5">
                        <label className="font-head text-sm font-semibold">E-mail</label>
                        <input type="email" value={email} onChange={e => setEmail(e.target.value)}
                            placeholder="example@gmail.com"
                            autoComplete="off"
                            className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
                    </div>
                    <div className="flex flex-col gap-1.5">
                        <label className="font-head text-sm font-semibold">Senha provisória</label>
                        <input type="password" value={password} onChange={e => setPassword(e.target.value)}
                            placeholder="Mín. 1 caractere"
                            autoComplete="new-password"
                            className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
                    </div>

                    <div className="flex gap-2.5 justify-end mt-1">
                        <button type="button" onClick={onClose} disabled={busy}
                            className="px-4 py-2 rounded-lg border border-border text-sm font-head font-semibold hover:bg-bg transition-colors cursor-pointer disabled:opacity-60">
                            Cancelar
                        </button>
                        <button type="submit" disabled={busy || !canSubmit}
                            className="px-4 py-2 rounded-lg bg-accent text-white text-sm font-head font-semibold hover:brightness-110 transition-[filter] cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:brightness-100">
                            {busy ? 'Criando…' : 'Criar Coach'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}