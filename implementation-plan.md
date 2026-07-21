# Implementation Plan

This plan aligns with the [Execution Plan](WORKTHROUGH.md) and breaks each phase into concrete tasks.

## Phase 1 — Planning ✅

| Task | Output |
|------|--------|
| Analyze requirements | `requirements-analysis.md` |
| Define acceptance criteria | `acceptance-criteria.md` |
| Design architecture | `design-notes.md` |
| Model data | `data-model.md` |
| Map UI flows | `ui-flow.md` |
| Draft API contract | `api-contract.md` |
| Write implementation plan | This file |

**Commit:** `Initial Planning`

---

## Phase 2 — Backend Foundation

| Task | Details |
|------|---------|
| Create solution | `SupportTicket.sln` with 4 projects |
| Configure DI | Register services in `Program.cs` |
| Add EF Core | `ApplicationDbContext` in Infrastructure |
| Add Swagger | Swashbuckle with XML comments |
| Add logging | Structured request logging |
| Exception middleware | Global handler with JSON error responses |

**Commit:** `Backend Setup`

---

## Phase 3 — Database

| Task | Details |
|------|---------|
| Define entities | User, Ticket, Comment in Domain |
| Configure relationships | Fluent API in Infrastructure |
| Create enums | TicketStatus, TicketPriority |
| Seed users | 3 users minimum |
| Seed tickets | 5 sample tickets with varied statuses |
| Initial migration | `dotnet ef migrations add InitialCreate` |

**Commit:** `Database Setup`

---

## Phase 4 — Backend Features

| Task | Endpoint |
|------|----------|
| List tickets | `GET /api/tickets` |
| Get ticket | `GET /api/tickets/{id}` |
| Create ticket | `POST /api/tickets` |
| Update ticket | `PUT /api/tickets/{id}` |
| List comments | `GET /api/tickets/{id}/comments` |
| Add comment | `POST /api/tickets/{id}/comments` |
| Search | `?search=` query param |
| Status filter | `?status=` query param |
| List users | `GET /api/users` |
| Validation | FluentValidation on DTOs |

**Commit:** `Ticket APIs`

---

## Phase 5 — State Machine

| Task | Details |
|------|---------|
| Transition validator | `StatusTransitionValidator` service |
| Status endpoint | `PATCH /api/tickets/{id}/status` |
| Domain exception | `InvalidStatusTransitionException` |
| Allowed transitions | Per state machine matrix in `data-model.md` |

**Commit:** `State Machine`

---

## Phase 6 — Frontend UI

| Task | Page/Component |
|------|----------------|
| Scaffold React app | Vite + TypeScript in `frontend/` |
| Layout + routing | React Router |
| Dashboard page | Status summary cards |
| Ticket list page | Table, search, filter |
| Create ticket page | Form with validation |
| Ticket detail page | Info, comments, status selector |
| Edit ticket page | Pre-populated form |
| Shared components | Badges, alerts, spinner |

**Commit:** `Frontend UI`

---

## Phase 7 — Integration

| Task | Details |
|------|---------|
| API client module | Axios/fetch wrapper with error handling |
| Connect all pages | Wire forms and lists to API |
| Loading states | Spinners during fetch |
| Error display | Parse API validation errors |
| CORS | Configure API for frontend origin |

**Commit:** `Frontend Integration`

---

## Phase 8 — Testing

| Test Type | Coverage |
|-----------|----------|
| Unit: StatusTransitionValidator | All valid and invalid transitions |
| Unit: Ticket validators | Required fields, max lengths |
| Integration: CRUD | Create, read, update tickets |
| Integration: Comments | Add and list comments |
| Integration: Search/Filter | Query param behavior |
| Integration: State machine | All AC-14 through AC-22 |

**Commit:** `Tests`

---

## Phase 9 — Documentation

| File | Content |
|------|---------|
| `test-strategy.md` | Test approach and coverage |
| `test-results.md` | Test run output |
| `debugging-notes.md` | Issues encountered |
| `code-review-notes.md` | Self-review findings |
| `review-fixes.md` | Fixes applied |
| `reflection.md` | Lessons learned |
| `final-ai-usage-summary.md` | AI tool usage summary |
| `pr-description.md` | PR summary |
| `ai-prompts/` | Prompt history per phase |

**Commit:** `Documentation`

---

## Phase 10 — Finalization

| Task | Details |
|------|---------|
| Complete README | Setup, run, test instructions |
| Remove secrets | Verify no credentials in repo |
| Environment variables | Document required config |
| Verify migrations | Fresh DB setup works |
| Verify seed data | Users and tickets present |
| Run all tests | All green |
| Verify Swagger | Endpoints documented |
| Git cleanup | `.gitignore`, no build artifacts |
| PR description | Final submission summary |

**Commit:** `Final Submission`

---

## Estimated Timeline

| Phase | Estimate |
|-------|----------|
| 1 — Planning | 1 session |
| 2 — Backend Foundation | 1 session |
| 3 — Database | 1 session |
| 4 — Backend Features | 1–2 sessions |
| 5 — State Machine | 1 session |
| 6 — Frontend UI | 1–2 sessions |
| 7 — Integration | 1 session |
| 8 — Testing | 1–2 sessions |
| 9 — Documentation | 1 session |
| 10 — Finalization | 1 session |

## Parallel Work Opportunities

- Documentation Agent can draft `test-strategy.md` and `debugging-notes.md` templates during Phases 2–7
- AI Workflow Agent can log prompts to `ai-prompts/` throughout
- Frontend UI scaffolding (Phase 6) can start once API contract is finalized (end of Phase 1)
