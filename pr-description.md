# Pull Request Description

## Title

`feat: Support Ticket Management System — Full Stack Assessment`

## Summary

This PR delivers a complete Support Ticket Management System built as a practical assessment project. It includes a .NET 8 Clean Architecture backend, SQL Server database with seed data, a React + TypeScript frontend, and 70 passing automated tests.

## What's Included

### Backend
- Ticket CRUD with FluentValidation
- Search (`?search=`) and status filter (`?status=`)
- Comment threads on tickets
- Strict status state machine (`Open → InProgress → Resolved → Closed`, with `Cancelled` paths)
- Dashboard summary endpoint
- Swagger at `/swagger`
- Serilog logging and global exception middleware

### Frontend
- Dashboard with status summary cards and recent tickets
- Ticket list with debounced search and status filter
- Create, edit, and detail pages with comments and status transitions
- Polished UI with single global "New Ticket" CTA
- API health banner and user-friendly error display

### Tests
- 39 unit tests (validators, state machine)
- 31 integration tests (full API via HTTP)
- All acceptance criteria AC-1–35 verified

### Documentation
- Requirements, design, API contract, data model
- Test strategy, results, debugging notes
- Code review, reflection, AI usage summary
- Phase-by-phase prompt history

## Test Plan

- [x] `dotnet test src/SupportTicket.sln` — 70/70 passing
- [x] API health check at `GET /api/health`
- [x] Swagger UI loads at `/swagger`
- [x] Dashboard shows ticket counts by status
- [x] Ticket list search and filter work
- [x] Create ticket → appears in list
- [x] Edit ticket → changes persist
- [x] Status transitions follow state machine rules
- [x] Comments can be added and viewed
- [x] Invalid transitions show error messages
- [x] No duplicate Create Ticket buttons in UI

## Setup

```bash
# Backend
dotnet run --project src/SupportTicket.API/SupportTicket.API.csproj --launch-profile http

# Frontend
cd frontend && npm install && npm run dev
```

- API: http://localhost:5172
- UI: http://localhost:5173

## Notes

- No authentication (assessment scope)
- LocalDB for development database
- AI-assisted development with full prompt history in `ai-prompts/`
