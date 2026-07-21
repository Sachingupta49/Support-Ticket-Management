# UI Flow

## Site Map

```text
/ (Dashboard)
├── /tickets (Ticket List)
│   ├── /tickets/new (Create Ticket)
│   └── /tickets/:id (Ticket Detail)
│       └── /tickets/:id/edit (Edit Ticket)
```

## Page Descriptions

### Dashboard (`/`)

**Purpose:** At-a-glance overview of ticket workload.

**Content:**
- Summary cards: count per status (Open, In Progress, Resolved, Closed, Cancelled)
- Quick link to create new ticket
- Recent tickets table (last 5 updated)

**Actions:**
- Click status card → navigate to filtered ticket list
- Click ticket row → navigate to detail

---

### Ticket List (`/tickets`)

**Purpose:** Browse, search, and filter all tickets.

**Components:**
- Search input (debounced, 300ms)
- Status filter dropdown (All, Open, In Progress, Resolved, Closed, Cancelled)
- Data table: ID, Title, Status, Priority, Assignee, Updated date
- "Create Ticket" button

**Actions:**
- Type in search → API call with `?search=`
- Select status filter → API call with `?status=`
- Click row → navigate to `/tickets/:id`
- Click "Create Ticket" → navigate to `/tickets/new`

---

### Create Ticket (`/tickets/new`)

**Purpose:** Submit a new support ticket.

**Form fields:**
| Field | Type | Validation |
|-------|------|------------|
| Title | Text input | Required, max 200 chars |
| Description | Textarea | Required |
| Priority | Select | Required (Low/Medium/High/Critical) |
| Assignee | Select | Required (from user list) |

**Actions:**
- Submit → `POST /api/tickets` → redirect to detail on success
- Cancel → navigate back to list
- Validation errors → inline field messages

---

### Ticket Detail (`/tickets/:id`)

**Purpose:** View full ticket information and conversation.

**Sections:**
1. **Header:** Title, status badge, priority badge, assignee
2. **Details:** Description, created/updated timestamps
3. **Status Actions:** Dropdown or buttons showing only valid next statuses
4. **Comments:** Chronological list with author and timestamp
5. **Add Comment:** Textarea + submit button

**Actions:**
- Change status → `PATCH /api/tickets/:id/status`
- Add comment → `POST /api/tickets/:id/comments`
- Edit → navigate to `/tickets/:id/edit`
- Back → navigate to list

---

### Edit Ticket (`/tickets/:id/edit`)

**Purpose:** Update ticket metadata (not status — status changed on detail page).

**Form fields:** Same as Create, pre-populated.

**Actions:**
- Save → `PUT /api/tickets/:id` → redirect to detail
- Cancel → navigate to detail

---

## Component Map

| Component | Used On | Responsibility |
|-----------|---------|----------------|
| `Layout` | All pages | Nav bar, main content wrapper |
| `StatusBadge` | List, Detail | Color-coded status display |
| `PriorityBadge` | List, Detail | Color-coded priority display |
| `TicketTable` | Dashboard, List | Reusable ticket data table |
| `SearchInput` | List | Debounced search |
| `StatusFilter` | List | Status dropdown filter |
| `TicketForm` | Create, Edit | Shared form for create/edit |
| `StatusSelector` | Detail | Valid next-status options only |
| `CommentList` | Detail | Renders comment thread |
| `CommentForm` | Detail | Add comment input |
| `ErrorAlert` | All | Display API/validation errors |
| `LoadingSpinner` | All | Loading state indicator |

## User Flow Diagram

```text
[Dashboard] ──► [Ticket List] ──► [Create Ticket] ──► [Ticket Detail]
                      │                                      │
                      └──────────────────────────────────────┘
                                                             │
                                                      [Edit Ticket]
```

## Error Handling UX

| Scenario | User Experience |
|----------|-----------------|
| Network error | Toast/alert: "Unable to connect. Please try again." |
| 404 ticket | Redirect to list with "Ticket not found" message |
| Validation error | Inline field errors from API response |
| Invalid status transition | Alert with server message explaining allowed transitions |
| Loading | Spinner or skeleton on data-fetching pages |

## Responsive Notes

- Table scrolls horizontally on narrow screens
- Form fields stack vertically on mobile
- Assessment targets desktop-first; basic mobile usability sufficient
