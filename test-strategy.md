# Test Strategy

## Overview

The backend uses a two-layer test approach: **unit tests** for isolated business logic and **integration tests** for full API endpoint verification.

## Test Projects

| Project | Location | Purpose |
|---------|----------|---------|
| `SupportTicket.UnitTests` | `tests/SupportTicket.UnitTests/` | Validators, state machine rules |
| `SupportTicket.IntegrationTests` | `tests/SupportTicket.IntegrationTests/` | API endpoints via HTTP |

## Technology Stack

| Tool | Usage |
|------|-------|
| xUnit | Test framework |
| FluentAssertions | Readable assertions |
| WebApplicationFactory | Integration test host |
| EF Core InMemory | Isolated test database per fixture |

## Unit Test Coverage

| Component | Tests |
|-----------|-------|
| `StatusTransitionValidator` | Valid transitions, invalid transitions, same-status rejection |
| `CreateTicketRequestValidator` | Title, priority, assignee validation |
| `CreateCommentRequestValidator` | Body required, max length |
| `UpdateTicketRequestValidator` | Title, priority validation |
| `UpdateTicketStatusRequestValidator` | Valid/invalid status values |

## Integration Test Coverage

| Area | Tests |
|------|-------|
| Ticket CRUD | List, get, create, update, 404, validation |
| Search & Filter | `?search=`, `?status=`, combined |
| Comments | List, create, ordering, 404, validation |
| State Machine | All AC-14 through AC-22 transitions |
| Edge Cases | 404 on update/status, invalid status query param |
| Users & Dashboard | User list, dashboard summary, health check |

## Running Tests

```bash
dotnet test src/SupportTicket.sln
```

## Test Data

Integration tests use `CustomWebApplicationFactory` which seeds 3 users, 5 tickets (one per status), and 2 comments into an in-memory database isolated per test class.
