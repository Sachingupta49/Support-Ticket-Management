# Phase 5 — State Machine Prompt

**Date:** 2026-07-21

## User Prompt

```
deferred part in step 5 and show me a swagger UI of it so that I can test the APIs in browser then proceed to the next steps
```

## Agent Response Summary

1. Implemented `StatusTransitionValidator` with strict transition rules
2. Added `PATCH /api/tickets/{id}/status` endpoint
3. Invalid transitions return 400 with allowed next statuses
4. Started API with Swagger UI for browser testing

## Allowed Transitions

| From | To |
|------|----|
| Open | InProgress, Cancelled |
| InProgress | Resolved, Cancelled |
| Resolved | Closed |
| Closed | (none) |
| Cancelled | (none) |
