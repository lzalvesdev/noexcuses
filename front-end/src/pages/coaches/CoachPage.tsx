import { ChevronRight, Plus, Search, Trash2 } from "lucide-react";
import { useEffect, useState } from "react";
import { deleteCoach, getCoaches } from "../../api/coach";
import { Avatar } from "../../components/Avatar";
import { ConfirmModal } from "../../components/ConfirmModal";
import type { Coach } from "../../types";
import { showError, showSuccess } from "../../utils/toast";
import { CoachModal } from "./CoachModal";
import { CreateCoachModal } from "./CreateCoachModal";

export function CoachPage() {
    const [coaches, setCoaches] = useState<Coach[]>([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [createOpen, setCreateOpen] = useState(false);
    const [activeCoach, setActiveCoach] = useState<Coach | null>(null);
    const [delTarget, setDelTarget] = useState<Coach | null>(null);
    const [delBusy, setDelBusy] = useState(false);

    useEffect(() => {
        getCoaches()
            .then(setCoaches)
            .catch(() => showError('Erro ao carregar treinadores'))
            .finally(() => setLoading(false));
    }, []);

    const filtered = coaches.filter(c =>
        c.firstName.toLowerCase().includes(search.toLowerCase()) ||
        c.email.toLowerCase().includes(search.toLowerCase())
    );

    async function handleDelete() {
        if (!delTarget) return;
        setDelBusy(true);
        try {
            await deleteCoach(delTarget.id);
            setCoaches(p => p.filter(c => c.id !== delTarget.id));
            setDelTarget(null);
            setActiveCoach(null);
            showSuccess('Coach removido com sucesso');
        } catch {
            showError('Erro ao remover coach');
        } finally {
            setDelBusy(false);
        }
    }

    return (
        <div className="p-8 flex flex-col flex-1">
            <div className="flex items-center justify-between mb-5">
                <div className="flex items-center gap-2.5">
                    <h1 className="font-head text-[22px] font-bold tracking-tight">Treinadores</h1>
                    <span className="bg-bg text-muted text-[13px] font-semibold rounded-full px-2.5 py-0.5">
                        {coaches.length}
                    </span>
                </div>
                <button onClick={() => setCreateOpen(true)}
                    className="flex items-center gap-1.5 bg-accent text-white font-head font-semibold text-sm px-4 py-2 rounded-lg hover:brightness-110 transition-[filter] cursor-pointer">
                    <Plus size={15} />Novo Treinador
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
                    style={{ gridTemplateColumns: '1fr 190px 84px 56px' }}>
                    {['Treinador', 'Especialidades', 'Atletas', ''].map((h, i) => (
                        <span key={i} className="text-[11.5px] font-head font-bold text-muted uppercase tracking-wider">{h}</span>
                    ))}
                </div>

                {loading ? (
                    <div className="py-12 text-center text-muted text-sm">Carregando…</div>
                ) : filtered.length === 0 ? (
                    <div className="py-12 text-center text-muted text-sm">
                        {search ? 'Nenhum treinador encontrado.' : 'Nenhum treinador cadastrado ainda.'}
                    </div>
                ) : filtered.map((c, i) => (
                    <div key={c.id}
                        onClick={() => setActiveCoach(c)}
                        className="grid px-5 py-3.5 items-center hover:bg-bg transition-colors cursor-pointer"
                        style={{ gridTemplateColumns: '1fr 190px 84px 56px', borderTop: i > 0 ? '1px solid var(--color-border)' : 'none' }}>

                        <div className="flex items-center gap-3">
                            <Avatar name={c.firstName} />
                            <div>
                                <div className="font-head font-semibold text-sm">{c.firstName}</div>
                                {/* <div className="text-[12px] text-muted">{c.email}</div> */}
                            </div>
                            <ChevronRight size={13} className="mr-auto text-muted" />
                        </div>

                        <div className="flex flex-wrap gap-1">
                            {c.specialities.slice(0, 2).map(s => (
                                <span key={s.id}
                                    className="inline-flex items-center bg-[#dcfce7] text-[#166534] rounded-full text-[10.5px] font-head font-semibold"
                                    style={{ paddingInline: '0.6em', paddingBlock: '0.2em' }}>
                                    {s.description}
                                </span>
                            ))}
                            {c.specialities.length > 2 &&
                                <span className="text-[11.5px] text-muted">
                                    +{c.specialities.length - 2}
                                </span>
                            }
                            {c.specialities.length === 0 &&
                                <span className="text-[12px] text-muted italic">
                                    Nenhuma
                                </span>
                            }
                        </div>

                        <div className="font-head font-bold text-[15px]">
                            {c.athletes.length}
                            <span className="text-[11.5px] text-muted font-normal ml-1">atletas</span>
                        </div>

                        <div className="flex justify-end" onClick={e => e.stopPropagation()}>
                            <button onClick={() => setDelTarget(c)}
                                className="p-1.5 rounded-lg text-muted hover:bg-danger/10 hover:text-danger transition-colors cursor-pointer">
                                <Trash2 size={14} />
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            {createOpen && (
                <CreateCoachModal onClose={() => setCreateOpen(false)} onDone={c => setCoaches(p => [...p, c])} />
            )}

            {activeCoach && (
                <CoachModal
                    coach={activeCoach}
                    onClose={() => setActiveCoach(null)}
                    onDelete={() => setDelTarget(activeCoach)}
                />
            )}

            {delTarget && (
                <ConfirmModal
                    title="Remover Treinador"
                    description={<>Deseja remover <strong>{delTarget.firstName}</strong>? Os atletas vinculados também
                        serão desvinculados.</>}
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