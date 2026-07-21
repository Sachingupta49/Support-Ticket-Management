# Reflection

**Project:** Support Ticket Management System  
**Date:** 2026-07-21  
**Author:** Sachin Gupta (with Cursor AI assistance)

## What Went Well

1. **Phased delivery** — Breaking the build into 10 phases with confirmation gates kept scope manageable and produced a clear git history.
2. **Tests before frontend** — Having 63+ backend tests before React development caught API contract issues early and gave confidence during UI integration.
3. **Clean Architecture** — Separating domain rules (state machine) from infrastructure made unit testing straightforward.
4. **AI-assisted scaffolding** — Cursor Agent accelerated boilerplate (entities, validators, API controllers, React pages) while I focused on verification and decisions.
5. **Documentation-as-you-go** — Maintaining `WORKTHROUGH.md` and `ai-prompts/` per phase created an audit trail without a last-minute documentation crunch.

## Challenges Encountered

| Challenge | Lesson |
|-----------|--------|
| `WebApplicationFactory` host build failure | Keep `Program.cs` minimal; avoid wrapping `Build()` in try-catch |
| API DLL locked during tests | Stop running API process before `dotnet test` |
| Node.js not installed in environment | Verify toolchain prerequisites before frontend phase |
| ERR_CONNECTION_REFUSED in browser | Both API and Vite dev server must be running |
| Duplicate UI CTAs | Establish a single source of truth for global actions in the layout |

## What I Would Do Differently

1. **Add pagination early** — The ticket list will not scale without server-side paging.
2. **Introduce auth stub** — Even a simple API key or mock JWT would make assignee/comment ownership more realistic.
3. **Component library** — For a larger app, adopting a design system (e.g. shadcn/ui) would speed UI work; custom CSS was fine for this scope.
4. **E2E tests** — Playwright or Cypress would complement integration tests for critical UI flows.

## Skills Demonstrated

- .NET 8 Web API with Clean Architecture
- EF Core migrations and seeding
- FluentValidation and domain-driven state machines
- xUnit integration testing with `WebApplicationFactory`
- React + TypeScript SPA with API integration
- Structured AI-assisted development with prompt history

## Overall Assessment

The project successfully delivers all functional requirements: ticket CRUD, search/filter, comments, state machine lifecycle, dashboard, and a polished React UI. The combination of automated tests (70 passing) and manual browser verification provides solid confidence in correctness.
