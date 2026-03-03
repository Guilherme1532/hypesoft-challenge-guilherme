# Technical Decisions

Este documento resume as principais decisoes tecnicas tomadas durante o desafio.

## Contexto de Prazo

- Estrategia adotada: priorizar funcionalidades obrigatorias, ambiente reproduzivel, autenticacao real e testes de maior impacto.

## 1) Autenticacao com Keycloak + roles

- Decisao: usar Keycloak como provedor OIDC/OAuth2, com controle de acesso por roles no frontend e backend.
- Motivo: atender requisito de autenticacao centralizada e autorizacao por perfil (`user`, `manager`, `admin`).
- Trade-off: dependencia de configuracao do realm.
- Mitigacao: import automatico de realm via Docker (`infra/keycloak/realm-hypesoft.json`).

## 2) Estoque em lote no frontend (multiplos PATCH)

- Decisao: implementar atualizacao em lote disparando chamadas individuais para `PATCH /api/products/{id}/stock`.
- Motivo: entregar funcionalidade sem alterar contrato da API em fase final.
- Trade-off: mais requisicoes HTTP por operacao de lote.
- Evolucao futura: endpoint batch dedicado no backend para maior eficiencia e atomicidade.

## 3) Docker Compose com stack completa

- Decisao: subir frontend, backend, MongoDB, Keycloak e Mongo Express via `docker-compose`.
- Motivo: facilitar validacao da entrega em ambiente unico.
- Trade-off: primeira subida mais lenta por build local.
- Mitigacao: documentacao explicita no README (`docker compose up -d --build`).

## 4) Seed idempotente no startup da API

- Decisao: popular automaticamente 4 categorias e 8 produtos quando banco estiver vazio.
- Motivo: permitir demonstracao imediata sem preparacao manual de dados.
- Trade-off: seed no startup acopla bootstrap de dados a inicializacao da API.
- Evolucao futura: processo dedicado de bootstrap/migration.

## 5) Mongo standalone no Docker e transacoes EF

- Decisao: configurar `AutoTransactionBehavior.Never` no `DbContext`.
- Motivo: Mongo standalone (sem replica set) nao suporta transacoes do provider EF, bloqueando seed e saves.
- Trade-off: sem transacao automatica multi-operacao nesse ambiente.
- Evolucao futura: usar replica set para suporte completo a transacoes.

## 6) Prioridade de testes por impacto

- Backend:
  - foco em handlers, validators e dominio para elevar cobertura.
  - remocao de placeholder de teste.
- Frontend:
  - setup leve com Vitest + Testing Library.
  - smoke tests para auth/roles/schemas/forms.
- Motivo: maximizar confianca funcional dentro do prazo.
