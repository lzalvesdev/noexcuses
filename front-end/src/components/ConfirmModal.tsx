interface Props {
    title: string;
    description: React.ReactNode;
    confirmLabel?: string;
    confirmVariant?: 'danger' | 'primary';
    onConfirm: () => void;
    onCancel: () => void;
    busy: boolean;
}

export function ConfirmModal({
    title,
    description,
    confirmLabel = 'Confirmar',
    confirmVariant = 'primary',
    onConfirm,
    onCancel,
    busy
}: Props) {
    const confirmStyle = confirmVariant === 'danger'
        ? 'bg-danger hover:brightness-110'
        : 'bg-accent hover:brightness-110';

    return (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-xs z-50 flex items-center justify-center">
            <div className="bg-white rounded-2xl p-7 w-full max-w-[380px] shadow-[0_20px_60px_rgba(0,0,0,.2)]">
                <h3 className="font-head text-[17px] font-bold mb-2">{title}</h3>
                <p className="text-[#71717a] text-sm leading-relaxed mb-6">{description}</p>
                <div className="flex gap-2.5 justify-end">
                    <button onClick={onCancel} disabled={busy}
                        className="px-4 py-2 rounded-lg border border-[#e4e4e7] text-sm font-head font-semibold hover:bg-[#f4f4f5] transition-colors cursor-pointer disabled:opacity-60">
                        Cancelar
                    </button>
                    <button onClick={onConfirm} disabled={busy}
                        className={`px-4 py-2 rounded-lg text-white text-sm font-head font-semibold transition-[filter] cursor-pointer disabled:opacity-60 ${confirmStyle}`}>
                        {busy ? 'Aguarde…' : confirmLabel}
                    </button>
                </div>
            </div>
        </div>
    );
}