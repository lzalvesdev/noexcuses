import { Avatar } from "@/components/Avatar";
import { Divider } from "@/components/Divider";
import type { Coach } from "@/types";
import { Trash2, X } from "lucide-react";

export function CoachModal({ coach, onClose, onDelete }: {
    coach: Coach;
    onClose: () => void;
    onDelete: () => void;
}) {
    return (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-xs z-50 flex items-center justify-center"
            onClick={onClose}>
            <div className="bg-white rounded-2xl p-7 w-full max-w-[480px] shadow-[0_20px_60px_rgba(0,0,0,.2)]"
                onClick={e => e.stopPropagation()}>

                <div className="flex items-start justify-between mb-5">
                    <div className="flex items-center gap-3.5">
                        <Avatar name={coach.firstName} size={52} />
                        <div>
                            <div className="font-head text-[18px] font-bold tracking-tight">{coach.firstName}</div>
                            <div className="text-[13px] text-muted mt-0.5">{coach.email}</div>
                        </div>
                    </div>
                    <button onClick={onClose}
                        className="text-muted hover:text-sidebar p-1.5 rounded-lg hover:bg-bg transition-colors cursor-pointer">
                        <X size={18} />
                    </button>
                </div>

                <Divider label="Especialidades" />
                <div className="flex flex-wrap gap-1.5 min-h-[26px]">
                    {coach.specialities.length === 0
                        ? <span className="text-[13px] text-muted italic">Nenhuma especialidade cadastrada</span>
                        : coach.specialities.map(s => (
                            <span key={s.id}
                                className="inline-flex items-center bg-[#dcfce7] text-[#166534] rounded-full text-[12px] font-head font-semibold"
                                style={{ paddingInline: '0.6em', paddingBlock: '0.2em' }}>
                                {s.description}
                            </span>
                        ))
                    }
                </div>

                <Divider label={`Atletas (${coach.athletes.length})`} />
                {coach.athletes.length === 0
                    ? <div className="text-[13px] text-muted italic">Nenhum atleta vinculado</div>
                    : (
                        <div className="rounded-xl border border-border overflow-hidden max-h-[200px] overflow-y-auto">
                            {coach.athletes.map((a, i) => (
                                <div key={a.id} className="bg-white flex items-center gap-2.5 px-3.5 py-2.5"
                                    style={{ borderBottom: i < coach.athletes.length - 1 ? '1px solid var(--color-border)' : 'none' }}>
                                    <Avatar name={a.firstName} size={30} />
                                    <div className="flex flex-col">
                                        <span className="font-head font-semibold text-sm">{a.firstName}</span>
                                        <span className="text-[12px] text-muted">{a.email}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )
                }

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
            </div>
        </div>
    );
}