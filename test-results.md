# Test Results

**Date:** 2026-07-21  
**Command:** `dotnet test src/SupportTicket.sln`  
**Browser verification:** User confirmed UI tested in browser before Phase 8

## Summary

| Project | Passed | Failed | Total |
|---------|--------|--------|-------|
| SupportTicket.UnitTests | 39 | 0 | 39 |
| SupportTicket.IntegrationTests | 31 | 0 | 31 |
| **Total** | **70** | **0** | **70** |

## Result

```
Passed!  - Failed: 0, Passed: 39 - SupportTicket.UnitTests.dll
Passed!  - Failed: 0, Passed: 31 - SupportTicket.IntegrationTests.dll
```

## Acceptance Criteria Coverage

| AC | Description | Verified By |
|----|-------------|-------------|
| AC-1 | POST creates Open ticket | `TicketApiTests.CreateTicket_ValidRequest_Returns201WithOpenStatus` |
| AC-2 | GET list tickets | `TicketApiTests.GetTickets_ReturnsSeededTickets` |
| AC-3 | GET by id / 404 | `TicketApiTests.GetTicketById_*` |
| AC-4 | PUT updates ticket | `TicketApiTests.UpdateTicket_ValidRequest_Returns200` |
| AC-5 | Missing title → 400 | `TicketApiTests.CreateTicket_MissingTitle_Returns400` |
| AC-6 | Invalid priority → 400 | `TicketApiTests.CreateTicket_InvalidPriority_Returns400` |
| AC-7 | Search filter | `TicketApiTests.GetTickets_WithSearch_*` |
| AC-8 | Status filter | `TicketApiTests.GetTickets_WithStatusFilter_*` |
| AC-9 | Combined search+status | `TicketApiTests.GetTickets_WithSearchAndStatus_*` |
| AC-10 | POST comment | `CommentApiTests.CreateComment_ValidRequest_Returns201` |
| AC-11 | Comments ordered | `CommentApiTests.GetComments_ExistingTicket_*` |
| AC-12 | Comment 404 | `CommentApiTests.CreateComment_NotFoundTicket_Returns404` |
| AC-13 | Empty comment → 400 | `CommentApiTests.CreateComment_EmptyBody_Returns400` |
| AC-14–18 | Valid transitions | `StatusTransitionApiTests.*_Succeeds` |
| AC-19–22 | Invalid transitions | `StatusTransitionApiTests.*_Returns400` |
| AC-23–30 | Frontend flows | Manual browser verification + integration-verification.md |
| AC-31 | Swagger accessible | `UsersAndDashboardApiTests` + manual |
| AC-34 | All tests pass | This report (70/70) |

## Phase 8 Additions

| Test | Type |
|------|------|
| `UpdateTicketRequestValidatorTests` | Unit |
| `UpdateTicket_NotFound_Returns404` | Integration |
| `GetTickets_InvalidStatus_Returns400` | Integration |
| `ChangeStatus_NotFound_Returns404` | Integration |
| `ChangeStatus_InvalidStatusValue_Returns400` | Integration |
