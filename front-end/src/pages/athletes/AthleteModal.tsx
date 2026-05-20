import { useState } from "react";
import { X, Trash2, Pencil } from "lucide-react";
import { updateAthleteCoach } from "../../api/athlete";
import { showSuccess, showError } from "../../utils/toast";
import type { Athlete, Coach } from "../../types";
import { Avatar } from "../../components/Avatar";
import { Divider } from "../../components/Divider";

export function AthleteModal({ athlete, coaches, initialMode = 'view', onClose, onDelete, onCoachUpdated }: {
    athlete: Athlete;
    coaches: Coach[];
    initialMode?: 'view' | 'edit';
    onClose: () => void;
    onDelete: () => void;
    onCoachUpdated: (athleteId: string, newCoachId: string) => void;
}) {
    const [mode, setMode] = useState<'view' | 'edit'>(initialMode);
    const [selectedCoachId, setSelectedCoachId] = useState(athlete.coachId);
    const [busy, setBusy] = useState(false);

    const currentCoach = coaches.find(c => c.id === athlete.coachId);

    async function handleSave(e: React.FormEvent) {
        e.preventDefault();
        if (selectedCoachId === athlete.coachId) { setMode('view'); return; }
        setBusy(true);
        try {
            await updateAthleteCoach(athlete.id, selectedCoachId);
            onCoachUpdated(athlete.id, selectedCoachId);
            showSuccess('Coach atualizado com sucesso.');
            setMode('view');
        } catch {
            showError('Erro ao atualizar coach.');
        } finally {
            setBusy(false);
        }
    }

    return (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-xs z-50 flex items-center justify-center"
            onClick={onClose}>
            <div className="bg-white rounded-2xl p-7 w-full max-w-[440px] shadow-[0_20px_60px_rgba(0,0,0,.2)]"
                onClick={e => e.stopPropagation()}>

                <div className="flex items-start justify-between mb-5">
                    <div className="flex items-center gap-3.5">
                        <Avatar name={athlete.firstName} size={52} />
                        <div>
                            <div className="font-head text-[18px] font-bold tracking-tight">{athlete.firstName}</div>
                            <div className="text-[13px] text-muted mt-0.5">{athlete.email}</div>
                        </div>
                    </div>
                    <div className="flex items-center gap-1">
                        {mode === 'view' && (
                            <button onClick={() => setMode('edit')}
                                className="text-muted hover:text-sidebar p-1.5 rounded-lg hover:bg-bg transition-colors cursor-pointer">
                                <Pencil size={15} />
                            </button>
                        )}
                        <button onClick={onClose}
                            className="text-muted hover:text-sidebar p-1.5 rounded-lg hover:bg-bg transition-colors cursor-pointer">
                            <X size={18} />
                        </button>
                    </div>
                </div>

                <Divider label="Coach Responsável" />

                {mode === 'view' && (
                    <>
                        {currentCoach ? (
                            <div className="flex items-center gap-3 px-3.5 py-3 bg-bg border border-[#dfdfdf] rounded-xl">
                                <Avatar name={currentCoach.firstName} size={38} />
                                <div>
                                    <div className="font-head font-semibold text-sm">{currentCoach.firstName}</div>
                                    <div className="text-[12px] text-muted">{currentCoach.email}</div>
                                </div>
                            </div>
                        ) : (
                            <div className="text-[13px] text-muted italic">Sem coach vinculado</div>
                        )}

                        <div className="flex items-center justify-between mt-6">
                            <button onClick={onDelete}
                                className="flex items-center gap-1.5 px-3 py-1.5 rounded-lg bg-danger/10 text-danger text-[13px] font-head font-semibold hover:bg-danger/20 transition-colors cursor-pointer">
                                <Trash2 size={13} />Remover
                            </button>
                            <button onClick={onClose}
                                className="px-3 py-1.5 rounded-lg border border-border text-[13px] font-head font-semibold hover:bg-bg transition-colors cursor-pointer">
                                Fechar
                            </button>
                        </div>
                    </>
                )}

                {mode === 'edit' && (
                    <>
                        <div className="text-[13px] text-muted bg-bg rounded-lg px-3.5 py-2.5 mb-4 leading-relaxed">
                            Alterar o coach responsável por este atleta.
                        </div>
                        <form onSubmit={handleSave} className="flex flex-col gap-3">
                            <select value={selectedCoachId} onChange={e => setSelectedCoachId(e.target.value)}
                                className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors">
                                {coaches.map(c => (
                                    <option key={c.id} value={c.id}>{c.firstName} — {c.email}</option>
                                ))}
                            </select>

                            <div className="flex gap-2 justify-end mt-1">
                                <button type="button" disabled={busy}
                                    onClick={() => {
                                        if (initialMode === 'edit') { onClose(); }
                                        else { setMode('view'); setSelectedCoachId(athlete.coachId); }
                                    }}
                                    className="px-4 py-2 rounded-lg border border-border text-sm font-head font-semibold hover:bg-bg transition-colors cursor-pointer disabled:opacity-60">
                                    Cancelar
                                </button>
                                <button type="submit" disabled={busy}
                                    className="px-4 py-2 rounded-lg bg-accent text-white text-sm font-head font-semibold hover:brightness-110 transition-[filter] cursor-pointer disabled:opacity-60">
                                    {busy ? 'Salvando…' : 'Salvar'}
                                </button>
                            </div>
                        </form>
                    </>
                )}
            </div>
        </div>
    );
}