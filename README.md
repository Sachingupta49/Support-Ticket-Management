# Support Ticket Management System

A full-stack support ticket management application built as a practical assessment project.

**Repository:** https://github.com/Sachingupta49/Support-Ticket-Management  
**Candidate:** Sachin Gupta

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | .NET 8 Web API (Clean Architecture) |
| Frontend | React 18 + TypeScript + Vite |
| Database | SQL Server (LocalDB for development) |
| ORM | Entity Framework Core 8 |
| Testing | xUnit + FluentAssertions + WebApplicationFactory |
| API Docs | Swagger / OpenAPI |

## Features

- Ticket CRUD with search and status filtering
- Comment threads on tickets
- Strict status state machine lifecycle
- Dashboard with ticket summary by status
- Swagger API documentation
- Polished React UI with responsive layout

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0+ | `dotnet --version` |
| [Node.js](https://nodejs.org/) | 18+ LTS | Required for frontend |
| SQL Server LocalDB | — | Included with Visual Studio / SQL Express |

## Quick Start

### 1. Clone and restore

```bash
git clone https://github.com/Sachingupta49/Support-Ticket-Management.git
cd Support-Ticket-Management
dotnet restore src/SupportTicket.sln
```

### 2. Database setup

Apply migrations and seed data:

```bash
dotnet ef database update ^
  --project src/SupportTicket.Infrastructure/SupportTicket.Infrastructure.csproj ^
  --startup-project src/SupportTicket.API/SupportTicket.API.csproj
```

> **Note:** Stop any running API process before running migrations or tests to avoid file-lock errors.

Seed data includes **3 users**, **5 tickets**, and **3 comments**.

### 3. Start the API

```bash
dotnet run --project src/SupportTicket.API/SupportTicket.API.csproj --launch-profile http
```

| Resource | URL |
|----------|-----|
| API | http://localhost:5172 |
| Swagger | http://localhost:5172/swagger |
| Health | http://localhost:5172/api/health |

### 4. Start the frontend

```bash
cd frontend
npm install
npm run dev
```

UI: http://localhost:5173

The Vite dev server proxies `/api` requests to the backend automatically.

## Running Tests

```bash
dotnet test src/SupportTicket.sln
```

| Project | Tests |
|---------|-------|
| SupportTicket.UnitTests | 39 |
| SupportTicket.IntegrationTests | 31 |
| **Total** | **70** |

## Project Structure

```
Support-Ticket-Management/
├── src/
│   ├── SupportTicket.API/          # Controllers, middleware, Program.cs
│   ├── SupportTicket.Application/    # Services, validators, DTOs
│   ├── SupportTicket.Domain/         # Entities, enums
│   └── SupportTicket.Infrastructure/ # EF Core, repositories, migrations
├── tests/
│   ├── SupportTicket.UnitTests/
│   └── SupportTicket.IntegrationTests/
├── frontend/                         # React + Vite SPA
├── ai-prompts/                       # AI prompt history per phase
└── *.md                              # Assessment documentation
```

## Configuration

### Backend

Default connection string in `src/SupportTicket.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupportTicketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Override via environment variable (recommended for non-local environments):

```bash
set ConnectionStrings__DefaultConnection=Server=...;Database=...;User Id=...;Password=...;
```

For local development overrides, copy the example file:

```bash
copy src\SupportTicket.API/appsettings.Development.json.example src/SupportTicket.API/appsettings.Development.json
```

`appsettings.Development.json` is gitignored and will not be committed.

### Frontend

Optional `.env` file (see `frontend/.env.example`):

```bash
# Only needed if not using Vite proxy
VITE_API_BASE_URL=http://localhost:5172/api
```

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/health` | Health check |
| GET | `/api/tickets` | List tickets (`?search=`, `?status=`) |
| GET | `/api/tickets/{id}` | Get ticket |
| POST | `/api/tickets` | Create ticket |
| PUT | `/api/tickets/{id}` | Update ticket |
| PATCH | `/api/tickets/{id}/status` | Change status (state machine) |
| GET | `/api/tickets/{id}/comments` | List comments |
| POST | `/api/tickets/{id}/comments` | Add comment |
| GET | `/api/users` | List users |
| GET | `/api/dashboard/summary` | Dashboard counts |

## Project Status

| Phase | Status |
|-------|--------|
| Planning | ✅ Complete |
| Backend Foundation | ✅ Complete |
| Database | ✅ Complete |
| Backend Features | ✅ Complete |
| State Machine | ✅ Complete |
| Frontend | ✅ Complete |
| Integration | ✅ Complete |
| Testing | ✅ Complete (70/70) |
| Documentation | ✅ Complete |
| Finalization | ✅ Complete |

See [WORKTHROUGH.md](WORKTHROUGH.md) for detailed phase-by-phase progress.

## Documentation

| Document | Description |
|----------|-------------|
| [requirements-analysis.md](requirements-analysis.md) | Functional and non-functional requirements |
| [acceptance-criteria.md](acceptance-criteria.md) | Testable acceptance criteria (AC-1–38) |
| [design-notes.md](design-notes.md) | Architecture and design decisions |
| [data-model.md](data-model.md) | Entity relationships and schema |
| [api-contract.md](api-contract.md) | REST API specification |
| [test-strategy.md](test-strategy.md) | Test approach and coverage |
| [test-results.md](test-results.md) | Latest test run output |
| [code-review-notes.md](code-review-notes.md) | Self-review findings |
| [reflection.md](reflection.md) | Lessons learned |
| [final-ai-usage-summary.md](final-ai-usage-summary.md) | AI tool usage summary |
| [pr-description.md](pr-description.md) | Final submission summary |

## Troubleshooting

| Issue | Solution |
|-------|----------|
| `ERR_CONNECTION_REFUSED` on port 5172/5173 | Start both API and frontend dev servers |
| `MSB3027` DLL locked during build/test | Stop the running `SupportTicket.API` process |
| `npm` not recognized | Install Node.js LTS and restart terminal |
| Database not found | Run `dotnet ef database update` (see step 2) |
| Swagger not loading | Ensure API started with `--launch-profile http` |

## License

Assessment project — not for production use.
