# Acceptance Criteria

## Ticket CRUD

- [x] **AC-1:** `POST /api/tickets` creates a ticket with status `Open` and returns `201` with the created resource
- [x] **AC-2:** `GET /api/tickets` returns a paginated or full list of tickets
- [x] **AC-3:** `GET /api/tickets/{id}` returns ticket details or `404` if not found
- [x] **AC-4:** `PUT /api/tickets/{id}` updates mutable fields and returns `200`
- [x] **AC-5:** Creating a ticket without a title returns `400` with validation errors
- [x] **AC-6:** Creating a ticket with invalid priority returns `400`

## Search and Filter

- [x] **AC-7:** `GET /api/tickets?search={term}` returns tickets matching title or description (case-insensitive)
- [x] **AC-8:** `GET /api/tickets?status={status}` returns only tickets with that status
- [x] **AC-9:** Search and status filter can be combined

## Comments

- [x] **AC-10:** `POST /api/tickets/{id}/comments` adds a comment and returns `201`
- [x] **AC-11:** `GET /api/tickets/{id}/comments` returns comments ordered by creation time ascending
- [x] **AC-12:** Adding a comment to a non-existent ticket returns `404`
- [x] **AC-13:** Empty comment body returns `400`

## State Machine

- [x] **AC-14:** `PATCH /api/tickets/{id}/status` with `Open → In Progress` succeeds
- [x] **AC-15:** `In Progress → Resolved` succeeds
- [x] **AC-16:** `Resolved → Closed` succeeds
- [x] **AC-17:** `Open → Cancelled` succeeds
- [x] **AC-18:** `In Progress → Cancelled` succeeds
- [x] **AC-19:** `Open → Resolved` returns `400` with transition error
- [x] **AC-20:** `Resolved → Open` returns `400` with transition error
- [x] **AC-21:** `Closed → In Progress` returns `400` with transition error
- [x] **AC-22:** `Cancelled → Open` returns `400` with transition error

## Frontend

- [x] **AC-23:** Dashboard displays ticket counts grouped by status
- [x] **AC-24:** Ticket list supports search input with debounced API call
- [x] **AC-25:** Ticket list supports status filter dropdown
- [x] **AC-26:** Create ticket form validates required fields before submit
- [x] **AC-27:** Ticket detail shows comments and allows adding new comments
- [x] **AC-28:** Edit ticket page loads existing data and saves changes
- [x] **AC-29:** Status selector only shows valid next statuses for current state
- [x] **AC-30:** API errors display user-friendly messages

## Infrastructure

- [x] **AC-31:** Swagger UI is accessible at `/swagger`
- [x] **AC-32:** Database migrations apply cleanly on fresh database
- [x] **AC-33:** Seed data includes at least 3 users and 5 sample tickets
- [x] **AC-34:** All unit and integration tests pass
- [x] **AC-35:** No secrets committed to repository

## Documentation

- [x] **AC-36:** All assessment markdown files are complete
- [x] **AC-37:** README includes setup and run instructions
- [x] **AC-38:** AI prompt history captured in `ai-prompts/`
