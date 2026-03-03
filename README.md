# Hypesoft Challenge - Sistema de Gestao de Produtos

Projeto full stack para gestao de produtos com autenticacao Keycloak, dashboard de estoque e CRUD completo de produtos/categorias.

## Visao Geral

A solucao foi implementada com:
- Backend em **.NET 9** (Clean Architecture + CQRS/MediatR + FluentValidation)
- Frontend em **React 18 + TypeScript + Vite**
- Banco **MongoDB**
- Autenticacao/autorizacao com **Keycloak** (OIDC/OAuth2)
- Stack completa via **Docker Compose**

## Stack Tecnologica

### Frontend
- React 18 + TypeScript
- Vite
- TailwindCSS + shadcn/ui
- TanStack Query
- React Hook Form + Zod
- Recharts
- Vitest + Testing Library

### Backend
- .NET 9
- Clean Architecture + CQRS (MediatR)
- FluentValidation
- AutoMapper
- MongoDB (EF Core Mongo provider)
- Serilog
- xUnit + FluentAssertions + Moq

### Infra
- Docker + Docker Compose
- MongoDB + Mongo Express
- Keycloak (import automatico de realm)

## Funcionalidades Implementadas

### Produtos
- Criar, listar, editar e excluir produtos
- Busca por nome
- Filtro por categoria
- Paginacao

### Categorias
- Criar, listar, editar e excluir categorias
- Associacao com produtos

### Estoque
- Atualizacao manual individual de estoque
- Atualizacao em lote (selecionando multiplos produtos)
- Lista de produtos com estoque baixo

### Dashboard
- Total de produtos
- Valor total em estoque
- Lista de baixo estoque
- Grafico de produtos por categoria

### Autenticacao e Autorizacao
- Login via Keycloak
- Logout integrado
- Protecao de rotas no frontend
- Controle por roles (`user`, `manager`, `admin`)

### UX/UI
- Layout responsivo (desktop/mobile)
- Menu hamburguer no mobile
- Toasts e dialogos de confirmacao
- Pagina de configuracoes:
  - Dados da conta
  - Tema (claro/escuro/sistema)
  - Densidade de layout

## Execucao Rapida (Docker)

### 1. Pre-requisitos
- Docker Desktop

### 2. Variaveis de ambiente
Crie `.env` na raiz com base no `.env.example`:

```env
MONGODB_USER=admin
MONGODB_PASSWORD=admin123
KEYCLOAK_ADMIN=admin
KEYCLOAK_ADMIN_PASSWORD=admin123
```

### 3. Subir aplicacao
```bash
docker compose up -d --build
```

### 4. URLs
- Frontend: http://localhost:3000
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health
- Keycloak: http://localhost:8080
- Mongo Express: http://localhost:8081

## Demonstracao

- A validacao funcional completa pode ser feita seguindo:
  - `docs/DEMO_GUIDE.md`
- Observacao: por restricao de tempo, a entrega esta acompanhada de guia detalhado de execucao e validacao em vez de video.
- Decisoes tecnicas resumidas:
  - `docs/DECISIONS.md`

## Keycloak (Import Automatico)

O projeto importa automaticamente o realm em:
- `infra/keycloak/realm-hypesoft.json`

Itens importados:
- Realm: `hypesoft`
- Client: `frontend`
- Roles: `user`, `manager`, `admin`
- Usuarios de exemplo:
  - `app_user` / `Pass@123`
  - `app_manager` / `Pass@123`
  - `app_admin` / `Pass@123`

## Seed de Banco

Ao iniciar a API com banco vazio, o projeto popula dados minimos automaticamente:
- 4 categorias
- 8 produtos

Observacao tecnica:
- Foi configurado `AutoTransactionBehavior.Never` no `DbContext` para suportar Mongo standalone no Docker.

## Reset de Ambiente

### Manter dados (usuarios/keycloak e banco)
```bash
docker compose down
docker compose up -d
```

### Reset total (apaga tudo)
```bash
docker compose down -v
docker compose up -d --build
```

## Desenvolvimento Local (sem Docker para app)

### Frontend
```bash
cd frontend
npm install
npm run dev
```

### Backend
```bash
cd backend
dotnet restore
dotnet run --project Hypesoft.API
```

## Testes

### Backend
```bash
dotnet test backend/Hypesoft.sln
```

### Frontend
```bash
cd frontend
npm run test
```

## Estrutura de Pastas (resumo)

```text
backend/
  Hypesoft.API/
  Hypesoft.Application/
  Hypesoft.Domain/
  Hypesoft.Infrastructure/
  Hypesoft.Application.Tests/

frontend/
  src/components/
    ui/
    forms/
    charts/
    layout/
  src/pages/
  src/hooks/
  src/services/
  src/stores/
  src/types/
  src/lib/
```

## Principais Endpoints

### Produtos
- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `PATCH /api/products/{id}/stock`
- `GET /api/products/low-stock`

### Categorias
- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`
- `DELETE /api/categories/{id}`

### Dashboard
- `GET /api/dashboard`

## Commits

O projeto segue **Conventional Commits** (`feat`, `fix`, `test`, `docs`, `chore`, etc.).

## Melhorias Futuras (opcional)

- Endpoint backend para atualizacao de estoque em lote (single request)
- Testes de integracao da API
- E2E no frontend
- Otimizacao de bundle (code splitting)
- Nginx reverse proxy dedicado para producao

### Evolucao DDD

- Introduzir `ValueObjects` para regras de dominio mais explicitas (ex.: nome de produto, preco, estoque e IDs de referencia)
- Adotar `DomainEvents` para eventos importantes do dominio (ex.: produto criado, estoque atualizado)
- Refinar agregados e invariantes para reforcar consistencia de negocio em cenarios de atualizacao
- Criar `Domain Services` para regras que atravessam multiplas entidades
- Padronizar retorno de erros de dominio com `Result/Error` para reduzir fluxo baseado em excecoes
