# Demo Guide (Sem Video)

Este guia permite validar a aplicacao em poucos minutos, cobrindo os principais requisitos funcionais.

## 1. Subir Ambiente

```bash
docker compose up -d --build
```

Validar URLs:
- Frontend: `http://localhost:3000`
- API/Swagger: `http://localhost:5000/swagger`
- Keycloak: `http://localhost:8080`

## 2. Credenciais de Teste

Usuarios importados automaticamente no Keycloak:
- `app_user` / `Pass@123`
- `app_manager` / `Pass@123`
- `app_admin` / `Pass@123`

## 3. Roteiro Rapido de Validacao

### 3.1 Login e controle por role
1. Acesse o frontend e clique em `Entrar com Keycloak`.
2. Entre com `app_user`.
3. Confirme acesso ao Dashboard, Produtos e Categorias.
4. Confirme que criacao/edicao/exclusao ficam bloqueadas para perfil `user`.
5. Faca logout e entre com `app_manager` (ou `app_admin`).
6. Confirme que acoes de mutacao estao liberadas.

### 3.2 Dashboard
1. Abra `Dashboard`.
2. Verifique:
- total de produtos
- valor total em estoque
- baixo estoque
- grafico de produtos por categoria

### 3.3 Produtos
1. Abra `Produtos`.
2. Teste busca por nome e filtro por categoria.
3. Crie um novo produto.
4. Edite o produto criado.
5. Exclua o produto com confirmacao.
6. Atualize estoque individual via botao `Estoque`.
7. Atualize estoque em lote:
- selecione multiplos produtos
- informe valor em `Estoque em lote`
- clique em `Atualizar lote`.

### 3.4 Categorias
1. Abra `Categorias`.
2. Crie uma categoria.
3. Edite a categoria inline.
4. Exclua a categoria (com confirmacao).

### 3.5 Configuracoes
1. Abra `Configuracoes`.
2. Verifique dados da conta (nome, username, email, roles).
3. Teste troca de tema: claro/escuro/sistema.
4. Teste densidade: confortavel/compacta.
5. Teste link de gerenciamento da conta no Keycloak.

## 4. Validacao Tecnica

### Backend
```bash
dotnet test backend/Hypesoft.sln
```

### Frontend
```bash
cd frontend
npm run test
npm run build
```

## 5. Dados e Reset

- Seed automatico: 4 categorias e 8 produtos (quando banco vazio).
- `docker compose down` -> para containers e preserva volumes.
- `docker compose down -v` -> remove volumes (apaga dados de Mongo e Keycloak), exigindo novo `up`.
