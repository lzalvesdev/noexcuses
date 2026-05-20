import { useEffect, useState } from "react";
import { Plus, Search, Trash2, ChevronRight, Pencil } from "lucide-react";
import { getAthletes, deleteAthlete } from "../../api/athlete";
import { getCoaches } from "../../api/coach";
import { showSuccess, showError } from "../../utils/toast";
import type { Athlete, Coach } from "../../types";
import { ConfirmModal } from "../../components/ConfirmModal";
import { Avatar } from "../../components/Avatar";
import { AthleteModal } from "./AthleteModal";
import { CreateAthleteModal } from "./CreateAthleteModal";

export function AthletePage() {
    const [athleteMode, setAthleteMode] = useState<'view' | 'edit'>('view');
    const [athletes, setAthletes] = useState<Athlete[]>([]);
    const [coaches, setCoaches] = useState<Coach[]>([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [activeAthlete, setActiveAthlete] = useState<Athlete | null>(null);
    const [createOpen, setCreateOpen] = useState(false);
    const [delTarget, setDelTarget] = useState<Athlete | null>(null);
    const [delBusy, setDelBusy] = useState(false);

    useEffect(() => {
        Promise.all([getAthletes(), getCoaches()])
            .then(([a, c]) => { setAthletes(a); setCoaches(c); })
            .catch(() => showError('Erro ao carregar atletas.'))
            .finally(() => setLoading(false));
    }, []);

    const filtered = athletes.filter(a =>
        a.firstName.toLowerCase().includes(search.toLowerCase()) ||
        a.email.toLowerCase().includes(search.toLowerCase())
    );

    function handleCoachUpdated(athleteId: string, newCoachId: string) {
        setAthletes(p => p.map(a => a.id === athleteId ? { ...a, coachId: newCoachId } : a));
        setActiveAthlete(p => p ? { ...p, coachId: newCoachId } : null);
    }

    async function handleDelete() {
        if (!delTarget) return;
        setDelBusy(true);
        try {
            await deleteAthlete(delTarget.id);
            setAthletes(p => p.filter(a => a.id !== delTarget.id));
            setDelTarget(null);
            setActiveAthlete(null);
            showSuccess('Atleta removido com sucesso.');
        } catch {
            showError('Erro ao remover atleta.');
        } finally {
            setDelBusy(false);
        }
    }

    return (
        <div className="p-8 flex flex-col flex-1">
            <div className="flex items-center justify-between mb-5">
                <div className="flex items-center gap-2.5">
                    <h1 className="font-head text-[22px] font-bold tracking-tight">Atletas</h1>
                    <span className="bg-[#E2E5E9] text-muted text-[13px] font-semibold rounded-full px-2.5 py-0.5">
                        {athletes.length}
                    </span>
                </div>
                <button onClick={() => setCreateOpen(true)}
                    className="flex items-center gap-1.5 bg-accent text-white font-head font-semibold text-sm px-4 py-2 rounded-lg hover:brightness-110 transition-[filter] cursor-pointer">
                    <Plus size={15} />Novo Atleta
                </button>
            </div>

            <div className="relative w-[300px] mb-5">
                <Search size={14} className="absolute left-3 top-1/2 -translate-y-1/2 text-muted pointer-events-none" />
                <input value={search} onChange={e => setSearch(e.target.value)}
                    placeholder="Buscar por nome ou e-mail…"
                    className="w-full bg-white pl-8 pr-3 py-2 rounded-lg border border-border text-[13.5px] outline-none focus:border-accent transition-colors" />
            </div>

            <div className="bg-white rounded-xl border border-border overflow-hidden shadow-sm">
                <div className="grid px-5 py-2.5 border-b border-border bg-bg"
                    style={{ gridTemplateColumns: '1fr 200px 80px' }}>
                    {['Atleta', 'Coach', ''].map((h, i) => (
                        <span key={i} className="text-[11.5px] font-head font-bold text-muted uppercase tracking-wider">{h}</span>
                    ))}
                </div>

                {loading ? (
                    <div className="py-12 text-center text-muted text-sm">Carregando…</div>
                ) : filtered.length === 0 ? (
                    <div className="py-12 text-center text-muted text-sm">
                        {search ? 'Nenhum atleta encontrado.' : 'Nenhum atleta cadastrado ainda.'}
                    </div>
                ) : filtered.map((a, i) => (
                    <div key={a.id}
                        onClick={() => { setActiveAthlete(a); setAthleteMode('view'); }}
                        className="grid px-5 py-3.5 items-center hover:bg-bg transition-colors cursor-pointer"
                        style={{ gridTemplateColumns: '1fr 200px 80px', borderTop: i > 0 ? '1px solid var(--color-border)' : 'none' }}>

                        <div className="flex items-center gap-3">
                            <Avatar name={a.firstName} />
                            <div>
                                <div className="font-head font-semibold text-sm">{a.firstName}</div>
                                {/* <div className="text-[12px] text-muted">{a.email}</div> */}
                            </div>
                            <ChevronRight size={13} className="mr-auto text-muted" />
                        </div>

                        <div className="flex items-center gap-2.5">
                            {(() => {
                                const coach = coaches.find(c => c.id === a.coachId);
                                return coach ?
                                    <>
                                        <Avatar name={coach.firstName} size={24} /><span className="font-head text-sm">
                                            {coach.firstName}
                                        </span>
                                    </>
                                    : <span className="text-[13px] text-muted italic">Sem coach</span>;
                            })()}
                        </div>

                        <div className="flex justify-end gap-0.5" onClick={e => e.stopPropagation()}>
                            <button onClick={() => { setActiveAthlete(a); setAthleteMode('edit'); }}
                                className="p-1.5 rounded-lg text-muted hover:bg-muted/10 hover:text-sidebar transition-colors cursor-pointer">
                                <Pencil size={14} />
                            </button>
                            <button onClick={() => setDelTarget(a)}
                                className="p-1.5 rounded-lg text-muted hover:bg-danger/10 hover:text-danger transition-colors cursor-pointer">
                                <Trash2 size={14} />
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            {createOpen && (
                <CreateAthleteModal
                    coaches={coaches}
                    onClose={() => setCreateOpen(false)}
                    onDone={a => setAthletes(p => [...p, a])}
                />
            )}

            {activeAthlete && (
                <AthleteModal
                    athlete={activeAthlete}
                    coaches={coaches}
                    initialMode={athleteMode}
                    onClose={() => setActiveAthlete(null)}
                    onDelete={() => setDelTarget(activeAthlete)}
                    onCoachUpdated={handleCoachUpdated}
                />
            )}

            {delTarget && (
                <ConfirmModal
                    title="Remover Atleta"
                    description={<>Deseja remover <strong>{delTarget.firstName}</strong>?</>}
                    confirmLabel="Remover"
                    confirmVariant="danger"
                    onConfirm={handleDelete}
                    onCancel={() => setDelTarget(null)}
                    busy={delBusy}
                />
            )}
        </div>
    );
}