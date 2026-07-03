# TesteTwrt API

API REST construída com .NET 8 seguindo os princípios de Clean Architecture. O sistema gerencia **Clientes**, **Produtos** e **Pedidos** com controle de estoque e histórico de status dos pedidos.

---

## Arquitetura

A solução é dividida em 4 projetos:

```
src/
├── TesteTwrt.Domain          # Entidades, enums, exceções, interfaces de repositório
├── TesteTwrt.Application     # Use cases, DTOs de entrada/saída, FluentValidation
├── TesteTwrt.Infrastructure  # EF Core, MySQL, implementações dos repositórios
└── TesteTwrt.WebApi          # Controllers, configuração de DI, handler global de exceções
tests/
└── TesteTwrt.UnitTests       # Testes unitários com xUnit e Moq
```

**Fluxo de dependências:** `WebApi → Application → Domain ← Infrastructure`

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | .NET 8 / ASP.NET Core |
| ORM | Entity Framework Core 8 + Pomelo MySQL |
| Banco de dados | MySQL 8.0 |
| Validação | FluentValidation 11 |
| Documentação | Swagger (Swashbuckle) |
| Containers | Docker + Docker Compose |
| Testes | xUnit + Moq |

---

## Pré-requisitos

- [Docker](https://www.docker.com/) e Docker Compose
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (somente para desenvolvimento local)

---

## Executando com Docker

```bash
docker-compose up --build
```

A API estará disponível em `http://localhost:5000`.  
Swagger UI: `http://localhost:5000/swagger`

> O banco de dados sobe automaticamente. Não é necessário rodar migrations manualmente — o EF Core aplica o schema na inicialização.

---

## Executando Localmente (sem Docker)

1. Suba apenas o banco de dados:

```bash
docker-compose up db
```

2. Verifique a connection string em `src/TesteTwrt.WebApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=testeTwrtDb;User=root;Password=root;"
}
```

3. Execute a API:

```bash
cd src/TesteTwrt.WebApi
dotnet run
```

---

## Executando os Testes

```bash
dotnet test tests/TesteTwrt.UnitTests
```

---

## Endpoints

### Clientes

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/customers` | Criar um cliente |
| `GET` | `/api/customers` | Listar todos os clientes |
| `GET` | `/api/customers/{id}` | Buscar cliente por ID |
| `PATCH` | `/api/customers/{id}/deactivate` | Desativar um cliente |

### Produtos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/products` | Criar um produto |
| `GET` | `/api/products` | Listar todos os produtos |
| `GET` | `/api/products/{id}` | Buscar produto por ID |
| `PUT` | `/api/products/{id}` | Atualizar nome e descrição |
| `PATCH` | `/api/products/{id}/price` | Atualizar preço |
| `PATCH` | `/api/products/{id}/stock` | Definir quantidade em estoque |
| `PATCH` | `/api/products/{id}/activate` | Ativar um produto |
| `PATCH` | `/api/products/{id}/deactivate` | Desativar um produto |

### Pedidos

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/orders` | Criar um pedido |
| `GET` | `/api/orders` | Listar todos os pedidos |
| `GET` | `/api/orders/{id}` | Buscar pedido por ID (com itens e histórico) |
| `PATCH` | `/api/orders/{id}/status` | Alterar status do pedido |

#### Fluxo de Status do Pedido

```
Criado ──► Pago ──► Enviado
  │
  └──► Cancelado
```

> Cancelar um pedido devolve o estoque automaticamente aos produtos. Pedidos já enviados não podem ser cancelados.

---

## Exemplos de Requisição

**Criar Cliente**
```json
POST /api/customers
{
  "name": "João Silva",
  "email": "joao@example.com",
  "document": "123.456.789-09"
}
```

**Criar Pedido**
```json
POST /api/orders
{
  "customerId": "00000000-0000-0000-0000-000000000000",
  "items": [
    { "productId": "00000000-0000-0000-0000-000000000000", "quantity": 2 }
  ]
}
```

**Alterar Status do Pedido**
```json
PATCH /api/orders/{id}/status
{
  "newStatus": 4,
  "reason": "Cancelado a pedido do cliente"
}
```

> Valores de status: `1` = Criado, `2` = Pago, `3` = Enviado, `4` = Cancelado

---

## Tratamento de Erros

Todos os erros seguem um formato de resposta consistente:

```json
{ "error": "Mensagem descritiva do erro" }
```

| HTTP Status | Quando |
|---|---|
| `400 Bad Request` | Falha de validação ou violação de regra de negócio |
| `404 Not Found` | Recurso não encontrado |
| `500 Internal Server Error` | Erro inesperado no servidor |
