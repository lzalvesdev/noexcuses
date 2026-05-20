# NoExcusesFit

Este projeto surgiu da minha vontade de sair de tutoriais e construir algo real, de ponta a ponta. Simulando uma plataforma de gestao para coaches e atletas, com a preocupação de fazer cada parte do jeito certo: arquitetura que não vira bagunça conforme cresce, autenticação que funciona de verdade em produção e infraestrutura pensada desde o início.

---

## 🛠 Stack

| | Tecnologia |
|---|---|
| 🔧 Backend | .NET 9, ASP.NET Core, Dapper, SQL Server |
| 🔐 Auth | JWT, Refresh Token Rotation, Rate Limiting |
| 🎨 Frontend | React 19, TypeScript, Tailwind CSS |
| ☁️ Infra | AWS EC2, VPC, Docker, Nginx |

---

## Decisões Técnicas

**Backend**: construído utilizando arquitetura em camadas como Clean Architecture. Autenticação via JWT com rotação de refresh tokens e rate limiting nas rotas sensíveis. Um Worker Service roda em background para limpeza de refresh tokens descartados.

**Frontend** — React com TypeScript, com interceptors Axios tratando refresh de token e fila de requisições em caso de expiração simultânea.

---

## Infraestrutura AWS

```
Internet
    │
[EC2 #1 — subnet pública]
    ├── Nginx  (SSL, proxy reverso)
    ├── API .NET
    └── Worker .NET
    │
    │  rede privada VPC
    ▼
[EC2 #2 — subnet privada]
    └── SQL Server  (sem acesso externo)
```

O banco fica isolado em uma subnet privada, só a API consegue chegar nele via rede interna. Acesso para administração via SSM Session Manager, sem expor portas ao público.

---

## Rodando localmente

1. Crie um `.env` na raiz baseado no `.env.example` e defina as credenciais
2. Suba o banco pela primeira vez: `docker compose up sqlserver`
3. Execute o script `back-end/noexcusesfit.sql` para criar o schema
4. Suba tudo: `docker compose up --build`

Acesse em `http://localhost`. Requer apenas Docker instalado.

---