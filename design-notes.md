# Design Notes

## Architecture Overview

The solution follows **Clean Architecture** with four layers:

```text
┌─────────────────────────────────────────┐
│              SupportTicket.API           │  ← Controllers, Middleware, DI
├─────────────────────────────────────────┤
│         SupportTicket.Application        │  ← Services, DTOs, Validators
├─────────────────────────────────────────┤
│           SupportTicket.Domain           │  ← Entities, Enums, Interfaces
├─────────────────────────────────────────┤
│       SupportTicket.Infrastructure       │  ← EF Core, Repositories, Migrations
└─────────────────────────────────────────┘
```

Dependency rule: outer layers depend on inner layers; Domain has zero dependencies.

## Technology Stack

| Layer | Technology |
|-------|------------|
| API | .NET 8, ASP.NET Core Web API |
| Application | FluentValidation, AutoMapper (optional) |
| Domain | Plain C# classes and interfaces |
| Infrastructure | EF Core 8, SQL Server |
| Frontend | React 18, TypeScript, Vite |
| Testing | xUnit, FluentAssertions, WebApplicationFactory |
| API Docs | Swashbuckle (Swagger) |

## Key Design Decisions

### D-1: State Machine in Domain Layer

Status transitions are enforced by a `TicketStatusService` (or domain method on `Ticket`) in the Domain/Application layer. Controllers delegate to this service; they do not contain transition logic.

```text
Request → Controller → TicketService.ChangeStatus()
                              ↓
                     StatusTransitionValidator
                              ↓
                     Valid? → Update + Save
                     Invalid? → throw InvalidStatusTransitionException
```

### D-2: Repository Pattern via EF Core

`IApplicationDbContext` or specific repository interfaces abstract data access. Infrastructure implements with `ApplicationDbContext`.

### D-3: Global Exception Middleware

A single middleware catches exceptions and maps them to consistent JSON responses:

| Exception Type | HTTP Status |
|----------------|-------------|
| `NotFoundException` | 404 |
| `ValidationException` | 400 |
| `InvalidStatusTransitionException` | 400 |
| Unhandled | 500 |

### D-4: DTOs for API Boundary

Entities are never returned directly. Request/response DTOs in the Application layer prevent over-posting and decouple API from persistence model.

### D-5: Frontend State Management

React with component-local state and a thin API client module. No Redux for assessment scope — `useState`/`useEffect` with custom hooks (`useTickets`, `useTicket`).

### D-6: Configuration

Connection strings and settings via `appsettings.json` with environment variable overrides:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupportTicketDb;Trusted_Connection=True;"
  }
}
```

Production secrets via `ConnectionStrings__DefaultConnection` environment variable.

## API Error Response Format

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "Title": ["Title is required."]
  }
}
```

## Logging

- Serilog or built-in `ILogger` with structured logging
- Log levels: Information for requests, Warning for business rule violations, Error for unhandled exceptions
- Request logging middleware for HTTP method, path, status code, duration

## Folder Structure

```text
src/
├── SupportTicket.sln
├── SupportTicket.API/
├── SupportTicket.Application/
├── SupportTicket.Domain/
└── SupportTicket.Infrastructure/
frontend/
├── src/
│   ├── api/
│   ├── components/
│   ├── pages/
│   ├── hooks/
│   └── types/
tests/
├── SupportTicket.UnitTests/
└── SupportTicket.IntegrationTests/
database/
└── (reference scripts if needed)
ai-prompts/
└── (prompt history per phase)
```
