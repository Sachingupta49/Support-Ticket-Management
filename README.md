# Support Ticket Management System

A full-stack support ticket management application built as a practical assessment project.

## Tech Stack

- **Backend:** .NET 8 Web API (Clean Architecture)
- **Frontend:** React 18 + TypeScript + Vite
- **Database:** SQL Server (LocalDB for development)
- **Testing:** xUnit + FluentAssertions

## Features

- Ticket CRUD with search and status filtering
- Comment threads on tickets
- Strict status state machine lifecycle
- Dashboard with ticket summary
- Swagger API documentation

## Project Status

| Phase | Status |
|-------|--------|
| Planning | ✅ Complete |
| Backend Foundation | ✅ Complete |
| Database | ✅ Complete |
| Backend Features | ✅ Complete |
| State Machine | ✅ Complete |
| Backend Tests | ✅ Complete (63/63 passing) |
| Frontend | ✅ Complete |
| Integration | ⬜ Pending verification |
| Backend Features | ⬜ Pending |
| State Machine | ⬜ Pending |
| Frontend | ⬜ Pending |
| Integration | ⬜ Pending |
| Testing | ⬜ Pending |
| Documentation | ⬜ Pending |
| Finalization | ⬜ Pending |

See [WORKTHROUGH.md](WORKTHROUGH.md) for detailed progress tracking.

## Documentation

| Document | Description |
|----------|-------------|
| [requirements-analysis.md](requirements-analysis.md) | Functional and non-functional requirements |
| [acceptance-criteria.md](acceptance-criteria.md) | Testable acceptance criteria |
| [design-notes.md](design-notes.md) | Architecture and design decisions |
| [data-model.md](data-model.md) | Entity relationships and schema |
| [ui-flow.md](ui-flow.md) | Page flows and components |
| [api-contract.md](api-contract.md) | REST API specification |
| [implementation-plan.md](implementation-plan.md) | Phased build plan |

## Setup Instructions

### Backend

```bash
dotnet run --project src/SupportTicket.API/SupportTicket.API.csproj --launch-profile http
```

API: http://localhost:5172 | Swagger: http://localhost:5172/swagger

### Frontend

```bash
cd frontend
npm install
npm run dev
```

UI: http://localhost:5173

### Tests

```bash
dotnet test src/SupportTicket.sln
```

## License

Assessment project — not for production use.
