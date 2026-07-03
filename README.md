# TesteTwrt API

REST API built with .NET 8 following Clean Architecture principles. The system manages **Customers**, **Products**, and **Orders** with stock control and order status history.

---

## Architecture

The solution is divided into 4 projects:

```
src/
├── TesteTwrt.Domain          # Entities, enums, exceptions, repository interfaces
├── TesteTwrt.Application     # Use cases, input/output DTOs, FluentValidation
├── TesteTwrt.Infrastructure  # EF Core, MySQL, repository implementations
└── TesteTwrt.WebApi          # Controllers, DI setup, global exception handler
tests/
└── TesteTwrt.UnitTests       # xUnit unit tests with Moq
```

**Dependency flow:** `WebApi → Application → Domain ← Infrastructure`

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 8 / ASP.NET Core |
| ORM | Entity Framework Core 8 + Pomelo MySQL |
| Database | MySQL 8.0 |
| Validation | FluentValidation 11 |
| Documentation | Swagger (Swashbuckle) |
| Containers | Docker + Docker Compose |
| Tests | xUnit + Moq |

---

## Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (only for local development)

---

## Running with Docker

```bash
docker-compose up --build
```

The API will be available at `http://localhost:5000`.  
Swagger UI: `http://localhost:5000/swagger`

> The database starts automatically. No manual migration is needed — EF Core applies the schema on startup.

---

## Running Locally (without Docker)

1. Start only the database:

```bash
docker-compose up db
```

2. Update the connection string in `src/TesteTwrt.WebApi/appsettings.json` if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=testeTwrtDb;User=root;Password=root;"
}
```

3. Run the API:

```bash
cd src/TesteTwrt.WebApi
dotnet run
```

---

## Running Tests

```bash
dotnet test tests/TesteTwrt.UnitTests
```

---

## API Endpoints

### Customers

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/customers` | Create a customer |
| `GET` | `/api/customers` | List all customers |
| `GET` | `/api/customers/{id}` | Get customer by ID |
| `PATCH` | `/api/customers/{id}/deactivate` | Deactivate a customer |

### Products

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/products` | Create a product |
| `GET` | `/api/products` | List all products |
| `GET` | `/api/products/{id}` | Get product by ID |
| `PUT` | `/api/products/{id}` | Update name and description |
| `PATCH` | `/api/products/{id}/price` | Update price |
| `PATCH` | `/api/products/{id}/stock` | Set stock quantity |
| `PATCH` | `/api/products/{id}/activate` | Activate a product |
| `PATCH` | `/api/products/{id}/deactivate` | Deactivate a product |

### Orders

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/orders` | Create an order |
| `GET` | `/api/orders` | List all orders |
| `GET` | `/api/orders/{id}` | Get order by ID (with items and history) |
| `PATCH` | `/api/orders/{id}/status` | Change order status |

#### Order Status Flow

```
Created ──► Paid ──► Shipped
   │
   └──► Cancelled
```

> Cancelling an order automatically returns stock to the products. Shipped orders cannot be cancelled.

---

## Request Examples

**Create Customer**
```json
POST /api/customers
{
  "name": "João Silva",
  "email": "joao@example.com",
  "document": "123.456.789-09"
}
```

**Create Order**
```json
POST /api/orders
{
  "customerId": "00000000-0000-0000-0000-000000000000",
  "items": [
    { "productId": "00000000-0000-0000-0000-000000000000", "quantity": 2 }
  ]
}
```

**Change Order Status**
```json
PATCH /api/orders/{id}/status
{
  "newStatus": 4,
  "reason": "Customer requested cancellation"
}
```

> Status values: `1` = Created, `2` = Paid, `3` = Shipped, `4` = Cancelled

---

## Error Handling

All errors follow a consistent response format:

```json
{ "error": "Descriptive error message" }
```

| HTTP Status | When |
|---|---|
| `400 Bad Request` | Validation failure or business rule violation |
| `404 Not Found` | Resource not found |
| `500 Internal Server Error` | Unexpected server error |
