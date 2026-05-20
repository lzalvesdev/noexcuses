import { AlertCircle, Eye, EyeOff } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Login, Register } from "../api/auth";
import { useAuth } from "../context/AuthContext";
import { showSuccess } from "../utils/toast";

type Mode = 'login' | 'register';
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function MockCoachCard() {
	return (
		<div className="bg-white rounded-xl p-4 shadow-xl w-[260px]">
			<div className="flex items-center gap-2.5 mb-3">
				<div className="w-9 h-9 rounded-full bg-[#22c55e] flex items-center justify-center text-white font-head font-bold text-sm shrink-0">CM</div>
				<div>
					<div className="font-head font-bold text-[13px] text-[#1a1d2e]">Carlos Mendes</div>
					<div className="text-[11px] text-muted">carlos@noexcuses.com</div>
				</div>
			</div>
			<div className="flex gap-1.5 mb-3">
				{['Musculação', 'Funcional'].map(s => (
					<span key={s} className="bg-[#dcfce7] text-[#166534] rounded-full px-2 py-0.5 text-[10.5px] font-head font-semibold">{s}</span>
				))}
			</div>
			<div className="border-t border-[#ededee] pt-2.5 flex gap-4">
				<div>
					<div className="font-head font-black text-base text-[#1a1d2e]">3</div>
					<div className="text-[10px] text-muted uppercase tracking-wide">Atletas</div>
				</div>
				<div>
					<div className="font-head font-black text-base text-[#1a1d2e]">2</div>
					<div className="text-[10px] text-muted uppercase tracking-wide">Especialidades</div>
				</div>
			</div>
		</div>
	);
}

function MockAthleteList() {
	const rows = [
		{ init: 'JS', name: 'João Silva', color: '#3b82f6' },
		{ init: 'PC', name: 'Pedro Costa', color: '#f97316' },
		{ init: 'FA', name: 'Fernanda Alves', color: '#ec4899' },
	];
	return (
		<div className="bg-white rounded-xl overflow-hidden shadow-xl w-[220px]">
			<div className="px-3.5 py-2.5 border-b border-[#ededee] text-[11px] font-head font-bold tracking-widest uppercase text-muted">Atletas</div>
			{rows.map((r, i) => (
				<div key={i} className={`flex items-center gap-2.5 px-3.5 py-2 ${i < rows.length - 1 ? 'border-b border-[#f4f4f5]' : ''}`}>
					<div className="w-6 h-6 rounded-full flex items-center justify-center text-white font-head font-bold text-[9.5px] shrink-0"
						style={{ background: r.color }}>{r.init}</div>
					<div className="font-head font-semibold text-[12.5px] text-[#1f2937]">{r.name}</div>
				</div>
			))}
		</div>
	);
}

export function LoginPage() {
	const { login } = useAuth();
	const navigate = useNavigate();

	const [mode, setMode] = useState<Mode>('login');
	const [showPw, setShowPw] = useState(false);
	// const [registered, setRegistered] = useState(false);

	// login
	const [loginEmail, setLoginEmail] = useState('');
	const [loginPassword, setLoginPassword] = useState('');
	const [loginError, setLoginError] = useState('');
	const [loginPending, setLoginPending] = useState(false);

	// register
	const [regName, setRegName] = useState('');
	const [regEmail, setRegEmail] = useState('');
	const [regPassword, setRegPassword] = useState('');
	const [regError, setRegError] = useState('');
	const [regPending, setRegPending] = useState(false);

	const loginValid = emailRegex.test(loginEmail) && loginPassword.length >= 1;
	const regValid = regName.trim() !== '' && emailRegex.test(regEmail) && regPassword.length >= 1;

	function switchMode(m: Mode) {
		setMode(m);
		setLoginError('');
		setRegError('');
	}

	async function handleLogin(e: React.FormEvent) {
		e.preventDefault();
		setLoginError('');
		setLoginPending(true);
		try {
			const { user } = await Login(loginEmail, loginPassword);
			login(user);
			navigate('/');
		} catch {
			setLoginError('Email ou senha inválidos.');
		} finally {
			setLoginPending(false);
		}
	}

	async function handleRegister(e: React.FormEvent) {
		e.preventDefault();
		setRegError('');
		setRegPending(true);
		try {
			await Register(regName, regEmail, regPassword);
			showSuccess("Conta criada! Faça login para continuar", 5000)
			setLoginEmail(regEmail);
			setLoginPassword(regPassword);
			switchMode('login');
		} catch {
			setRegError('Erro ao criar conta. Verifique os dados.');
		} finally {
			setRegPending(false);
		}
	}

	return (
		<div className="min-h-screen flex items-center justify-center bg-[#f0f0f2] p-6 font-body">
			<div className="flex w-full max-w-[980px] min-h-[600px] rounded-2xl overflow-hidden shadow-[0_24px_80px_rgba(0,0,0,.13)]">

				{/* Esquerda — Form */}
				<div className="w-[420px] shrink-0 bg-white flex flex-col justify-center px-11 py-12">
					<h1 className="font-head text-2xl font-extrabold tracking-tight text-[#131520] mb-1.5">Bem-vindo de volta</h1>
					<p className="text-muted text-[13.5px] leading-relaxed mb-7">Insira seus dados para continuar.</p>

					{/* Tab toggle */}
					<div className="flex bg-[#f0f0f2] rounded-[10px] p-1 gap-0.5 mb-7">
						{(['login', 'register'] as Mode[]).map(m => (
							<button key={m} type="button" onClick={() => switchMode(m)}
								className={`flex-1 py-[7px] rounded-[7px] font-head font-semibold text-[13.5px] transition-all cursor-pointer ${mode === m ? 'bg-white text-[#1a1d2e] shadow-sm' : 'bg-transparent text-muted'}`}>
								{m === 'login' ? 'Entrar' : 'Criar conta'}
							</button>
						))}
					</div>

					{/* Login form */}
					{mode === 'login' && (
						<form key="login" onSubmit={handleLogin} className="flex flex-col gap-3.5 animate-fade-up">
							<div className="flex flex-col gap-1.5">
								<label className="font-head text-sm font-semibold">E-mail</label>
								<input type="email" placeholder="seu@email.com" autoComplete="email"
									value={loginEmail} onChange={e => setLoginEmail(e.target.value)}
									className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
							</div>
							<div className="flex flex-col gap-1.5">
								<label className="font-head text-sm font-semibold">Senha</label>
								<div className="relative">
									<input type={showPw ? 'text' : 'password'} placeholder="••••••••" autoComplete="current-password"
										value={loginPassword} onChange={e => setLoginPassword(e.target.value)}
										className="w-full pr-10 px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
									<button type="button" onClick={() => setShowPw(p => !p)}
										className="absolute right-3 top-1/2 -translate-y-1/2 text-muted cursor-pointer">
										{showPw ? <EyeOff size={15} /> : <Eye size={15} />}
									</button>
								</div>
							</div>
							{loginError && (
								<div className="flex items-center gap-1.5 text-danger text-sm">
									<AlertCircle size={14} />{loginError}
								</div>
							)}
							<button type="submit" disabled={loginPending || !loginValid}
								className="mt-1 py-[11px] rounded-[9px] bg-accent text-white font-head font-bold text-[14.5px] tracking-tight cursor-pointer hover:brightness-110 transition-[filter] disabled:opacity-40 disabled:cursor-not-allowed">
								{loginPending ? 'Entrando…' : 'Entrar'}
							</button>
						</form>
					)}

					{/* Register form */}
					{mode === 'register' && (
						<form key="register" onSubmit={handleRegister} className="flex flex-col gap-3.5 animate-fade-up">
							<div className="flex flex-col gap-1.5">
								<label className="font-head text-sm font-semibold">Nome completo</label>
								<input type="text" placeholder="Carlos Mendes" autoComplete="name"
									value={regName} onChange={e => setRegName(e.target.value)}
									className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
							</div>
							<div className="flex flex-col gap-1.5">
								<label className="font-head text-sm font-semibold">E-mail</label>
								<input type="email" placeholder="seu@email.com" autoComplete="email"
									value={regEmail} onChange={e => setRegEmail(e.target.value)}
									className="px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
							</div>
							<div className="flex flex-col gap-1.5">
								<label className="font-head text-sm font-semibold">Senha</label>
								<div className="relative">
									<input type={showPw ? 'text' : 'password'} placeholder="Mín. 8 caracteres" autoComplete="new-password"
										value={regPassword} onChange={e => setRegPassword(e.target.value)}
										className="w-full pr-10 px-3 py-2.5 rounded-lg border border-border text-sm outline-none focus:border-accent transition-colors" />
									<button type="button" onClick={() => setShowPw(p => !p)}
										className="absolute right-3 top-1/2 -translate-y-1/2 text-muted cursor-pointer">
										{showPw ? <EyeOff size={15} /> : <Eye size={15} />}
									</button>
								</div>
							</div>
							{regError && (
								<div className="flex items-center gap-1.5 text-danger text-sm">
									<AlertCircle size={14} />{regError}
								</div>
							)}
							<button type="submit" disabled={regPending || !regValid}
								className="mt-1 py-[11px] rounded-[9px] bg-accent text-white font-head font-bold text-[14.5px] tracking-tight cursor-pointer hover:brightness-110 transition-[filter] disabled:opacity-40 disabled:cursor-not-allowed">
								{regPending ? 'Criando…' : 'Criar conta'}
							</button>
						</form>
					)}

					<p className="mt-7 text-[12.5px] text-[#9ca3af] text-center">NoExcusesFit © 2026</p>
				</div>

				{/* Direita — Brand */}
				<div className="flex-1 relative overflow-hidden bg-[#0e1118] flex flex-col justify-between p-11">
					<div className="absolute inset-0 z-0" style={{
						backgroundImage: `
              repeating-linear-gradient(-52deg, transparent, transparent 32px, rgba(255,255,255,.025) 32px, rgba(255,255,255,.025) 33px),
              repeating-linear-gradient(-52deg, transparent, transparent 96px, rgba(255,255,255,.018) 96px, rgba(255,255,255,.018) 98px)`
					}} />
					<div className="absolute -top-[10%] -right-[5%] w-[340px] h-[340px] rounded-full bg-accent opacity-[.07] blur-[60px] z-0" />
					<div className="absolute -bottom-[5%] left-[10%] w-[200px] h-[200px] rounded-full bg-[#818cf8] opacity-[.06] blur-[50px] z-0" />

					<div className="relative z-10">
						<div className="inline-flex items-center gap-2 mb-6">
							<span className="w-1.5 h-1.5 rounded-full bg-accent block shadow-[0_0_8px_#4ade80]" />
							<span className="font-head font-bold text-[10.5px] tracking-[.14em] uppercase text-accent">Gestão de Atletas</span>
						</div>
						<div className="font-head font-black leading-[.92] tracking-[-0.055em] mb-6" style={{ fontSize: 'clamp(36px, 3.8vw, 52px)' }}>
							<div className="text-white">TREINE.</div>
							<div className="text-accent">GERENCIE.</div>
							<div className="text-white opacity-30">EVOLUA.</div>
						</div>
						<div className="flex items-center gap-3">
							<div className="w-5 h-0.5 bg-accent rounded shrink-0" />
							<p className="text-[#4b5563] text-[13px] leading-relaxed max-w-[280px]">
								Plataforma completa para quem leva o desempenho a sério.
							</p>
						</div>
					</div>

					<div className="relative z-10 flex-1 flex items-end pb-2">
						<div className="relative z-20 -rotate-2 mb-4">
							<MockCoachCard />
						</div>
						<div className="absolute right-0 bottom-16 z-30 rotate-[1.5deg]">
							<MockAthleteList />
						</div>
					</div>
				</div>

			</div>
		</div>
	);
}
