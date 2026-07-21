# Requirements Analysis

## Overview

Build a **Support Ticket Management System** that allows users to create, view, update, and track support tickets through a defined lifecycle. The system consists of a .NET Web API backend, a React frontend, and a SQL Server database.

## Functional Requirements

### FR-1: Ticket Management

| ID | Requirement |
|----|-------------|
| FR-1.1 | Users can create a ticket with title, description, priority, and assignee |
| FR-1.2 | Users can view a list of all tickets |
| FR-1.3 | Users can view ticket details including comments |
| FR-1.4 | Users can update ticket fields (title, description, priority, assignee) |
| FR-1.5 | Users can change ticket status only through allowed state transitions |
| FR-1.6 | Users can search tickets by title or description |
| FR-1.7 | Users can filter tickets by status |

### FR-2: Comments

| ID | Requirement |
|----|-------------|
| FR-2.1 | Users can add comments to a ticket |
| FR-2.2 | Comments display author name and timestamp |
| FR-2.3 | Comments are ordered chronologically |

### FR-3: Status Lifecycle (State Machine)

| ID | Requirement |
|----|-------------|
| FR-3.1 | New tickets start in **Open** status |
| FR-3.2 | Allowed transitions: Open → In Progress, In Progress → Resolved, Resolved → Closed, Open → Cancelled, In Progress → Cancelled |
| FR-3.3 | All other status transitions must be rejected with a clear error |

### FR-4: User Management

| ID | Requirement |
|----|-------------|
| FR-4.1 | System has pre-seeded users (agents) for assignment |
| FR-4.2 | Tickets reference an assignee from the user list |
| FR-4.3 | Comments reference the author user |

### FR-5: Frontend Pages

| ID | Requirement |
|----|-------------|
| FR-5.1 | Dashboard with ticket summary counts by status |
| FR-5.2 | Ticket list with search and status filter |
| FR-5.3 | Create ticket form |
| FR-5.4 | Ticket detail view with comments |
| FR-5.5 | Edit ticket form |

## Non-Functional Requirements

| ID | Requirement |
|----|-------------|
| NFR-1 | API documented via Swagger/OpenAPI |
| NFR-2 | Structured logging for API requests and errors |
| NFR-3 | Global exception handling with consistent error responses |
| NFR-4 | Input validation on all write endpoints |
| NFR-5 | Unit and integration test coverage for core business logic |
| NFR-6 | Clean Architecture separation of concerns |
| NFR-7 | Database migrations managed via EF Core |
| NFR-8 | No hardcoded secrets; configuration via environment variables |

## Out of Scope

- Full authentication/authorization (OAuth, JWT login flows)
- Email notifications
- File attachments on tickets
- Role-based access control beyond basic user assignment
- Real-time updates (WebSockets)
- Multi-tenancy

## Assumptions

- Single-tenant application for assessment purposes
- Seeded users are sufficient for demo and testing
- SQL Server LocalDB is acceptable for local development
- A default "current user" is selected in the frontend for comment authorship

## Risks and Mitigations

| Risk | Mitigation |
|------|------------|
| State machine logic scattered across layers | Centralize in domain service with unit tests |
| Frontend/backend contract drift | Define `api-contract.md` upfront and validate in integration tests |
| Migration issues on fresh clone | Document setup steps in README; include seed data |
