# Integration Verification

**Date:** 2026-07-21  
**Phase:** 7 — Frontend Integration

## Prerequisites

| Service | URL | Command |
|---------|-----|---------|
| API | http://localhost:5172 | `dotnet run --project src/SupportTicket.API --launch-profile http` |
| Frontend | http://localhost:5173 | `cd frontend && npm install && npm run dev` |

## Automated Verification

```bash
dotnet test src/SupportTicket.sln
```

**Result:** 63/63 tests passing

## Manual Integration Checklist

| # | Flow | Expected |
|---|------|----------|
| 1 | Open Dashboard | Status cards load with counts |
| 2 | Click status card | Navigates to filtered ticket list |
| 3 | Search tickets | Debounced results match title/description |
| 4 | Filter by status | Only matching tickets shown |
| 5 | Create ticket | Redirects to detail, status = Open |
| 6 | Submit empty title | Validation errors displayed |
| 7 | Edit ticket | Changes saved, reflected on detail |
| 8 | Change status (valid) | Status updates immediately |
| 9 | Change status (invalid) | Error message with allowed transitions |
| 10 | Add comment | Comment appears in list |
| 11 | Visit invalid ticket ID | Redirects to list with "Ticket not found" |
| 12 | Stop API, reload UI | Banner shows API unreachable message |

## Integration Points Verified

- [x] Vite proxy forwards `/api` → `localhost:5172`
- [x] CORS configured for `http://localhost:5173`
- [x] Loading spinners on all data-fetching pages
- [x] API validation errors mapped to form fields
- [x] Server errors displayed via ErrorAlert
- [x] Network failures show user-friendly message
- [x] 404 redirects with flash message
