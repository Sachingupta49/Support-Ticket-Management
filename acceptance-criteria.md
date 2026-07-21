# Acceptance Criteria

## Ticket CRUD

- [ ] **AC-1:** `POST /api/tickets` creates a ticket with status `Open` and returns `201` with the created resource
- [ ] **AC-2:** `GET /api/tickets` returns a paginated or full list of tickets
- [ ] **AC-3:** `GET /api/tickets/{id}` returns ticket details or `404` if not found
- [ ] **AC-4:** `PUT /api/tickets/{id}` updates mutable fields and returns `200`
- [ ] **AC-5:** Creating a ticket without a title returns `400` with validation errors
- [ ] **AC-6:** Creating a ticket with invalid priority returns `400`

## Search and Filter

- [ ] **AC-7:** `GET /api/tickets?search={term}` returns tickets matching title or description (case-insensitive)
- [ ] **AC-8:** `GET /api/tickets?status={status}` returns only tickets with that status
- [ ] **AC-9:** Search and status filter can be combined

## Comments

- [ ] **AC-10:** `POST /api/tickets/{id}/comments` adds a comment and returns `201`
- [ ] **AC-11:** `GET /api/tickets/{id}/comments` returns comments ordered by creation time ascending
- [ ] **AC-12:** Adding a comment to a non-existent ticket returns `404`
- [ ] **AC-13:** Empty comment body returns `400`

## State Machine

- [ ] **AC-14:** `PATCH /api/tickets/{id}/status` with `Open → In Progress` succeeds
- [ ] **AC-15:** `In Progress → Resolved` succeeds
- [ ] **AC-16:** `Resolved → Closed` succeeds
- [ ] **AC-17:** `Open → Cancelled` succeeds
- [ ] **AC-18:** `In Progress → Cancelled` succeeds
- [ ] **AC-19:** `Open → Resolved` returns `400` with transition error
- [ ] **AC-20:** `Resolved → Open` returns `400` with transition error
- [ ] **AC-21:** `Closed → In Progress` returns `400` with transition error
- [ ] **AC-22:** `Cancelled → Open` returns `400` with transition error

## Frontend

- [ ] **AC-23:** Dashboard displays ticket counts grouped by status
- [ ] **AC-24:** Ticket list supports search input with debounced API call
- [ ] **AC-25:** Ticket list supports status filter dropdown
- [ ] **AC-26:** Create ticket form validates required fields before submit
- [ ] **AC-27:** Ticket detail shows comments and allows adding new comments
- [ ] **AC-28:** Edit ticket page loads existing data and saves changes
- [ ] **AC-29:** Status selector only shows valid next statuses for current state
- [ ] **AC-30:** API errors display user-friendly messages

## Infrastructure

- [ ] **AC-31:** Swagger UI is accessible at `/swagger`
- [ ] **AC-32:** Database migrations apply cleanly on fresh database
- [ ] **AC-33:** Seed data includes at least 3 users and 5 sample tickets
- [ ] **AC-34:** All unit and integration tests pass
- [ ] **AC-35:** No secrets committed to repository

## Documentation

- [ ] **AC-36:** All assessment markdown files are complete
- [ ] **AC-37:** README includes setup and run instructions
- [ ] **AC-38:** AI prompt history captured in `ai-prompts/`
