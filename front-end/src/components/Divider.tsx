export function Divider({ label }: { label: string }) {
    return (
        <div className="flex items-center gap-2.5 mt-5 mb-3">
            <span className="text-[10px] font-head font-bold text-[#71717a] tracking-[.08em] uppercase
  whitespace-nowrap">{label}</span>
            <div className="flex-1 h-px bg-[#e4e4e7]" />
        </div>
    );
}