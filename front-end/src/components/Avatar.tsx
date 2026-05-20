const HUES = [142, 215, 28, 280, 55, 340, 185, 0];

interface Props {
    name: string;
    size?: number;
}

export function Avatar({ name, size = 36 }: Props) {
    const initials = name.split(' ').map(w => w[0]?.toUpperCase() ?? '').slice(0, 2).join('');
    const hue = HUES[Math.abs(name.charCodeAt(0)) % HUES.length];
    return (
        <div className="rounded-full flex items-center justify-center text-white font-head font-bold shrink-0"
            style={{ width: size, height: size, fontSize: size * 0.36, background: `hsl(${hue * 1.2}deg 55% 52%)` }}>
            {initials}
        </div>
    );
}