# API Contract

Base URL: `http://localhost:5000/api` (development)

All responses use `application/json`. Errors follow RFC 7807 Problem Details format.

---

## Users

### GET /users

Returns all users for assignee dropdowns.

**Response `200`:**

```json
[
  {
    "id": 1,
    "name": "Alice Agent",
    "email": "alice@support.com"
  }
]
```

---

## Tickets

### GET /tickets

List tickets with optional search and status filter.

**Query Parameters:**

| Param | Type | Description |
|-------|------|-------------|
| search | string | Case-insensitive match on title or description |
| status | string | Filter by status name (e.g. `Open`, `InProgress`) |

**Response `200`:**

```json
[
  {
    "id": 1,
    "title": "Cannot login to portal",
    "description": "User reports 401 error on login page.",
    "status": "Open",
    "priority": "High",
    "assigneeId": 1,
    "assigneeName": "Alice Agent",
    "createdAt": "2026-07-20T10:00:00Z",
    "updatedAt": "2026-07-20T10:00:00Z"
  }
]
```

---

### GET /tickets/{id}

**Response `200`:** Single ticket object (same shape as list item).

**Response `404`:** Ticket not found.

---

### POST /tickets

**Request:**

```json
{
  "title": "Cannot login to portal",
  "description": "User reports 401 error on login page.",
  "priority": "High",
  "assigneeId": 1
}
```

**Response `201`:** Created ticket object.

**Response `400`:** Validation errors.

---

### PUT /tickets/{id}

Update ticket metadata. Does not change status.

**Request:**

```json
{
  "title": "Updated title",
  "description": "Updated description.",
  "priority": "Medium",
  "assigneeId": 2
}
```

**Response `200`:** Updated ticket object.

**Response `404`:** Ticket not found.

**Response `400`:** Validation errors.

---

### PATCH /tickets/{id}/status

Change ticket status. Enforces state machine rules.

**Request:**

```json
{
  "status": "InProgress"
}
```

**Response `200`:** Updated ticket object.

**Response `400`:** Invalid transition (e.g. Open → Resolved).

**Response `404`:** Ticket not found.

**Valid status values:** `Open`, `InProgress`, `Resolved`, `Closed`, `Cancelled`

---

## Comments

### GET /tickets/{id}/comments

**Response `200`:**

```json
[
  {
    "id": 1,
    "ticketId": 1,
    "authorId": 2,
    "authorName": "Bob Agent",
    "body": "Investigating the auth logs.",
    "createdAt": "2026-07-20T11:00:00Z"
  }
]
```

**Response `404`:** Ticket not found.

---

### POST /tickets/{id}/comments

**Request:**

```json
{
  "authorId": 2,
  "body": "Investigating the auth logs."
}
```

**Response `201`:** Created comment object.

**Response `400`:** Validation errors (empty body).

**Response `404`:** Ticket not found.

---

## Dashboard

### GET /dashboard/summary

**Response `200`:**

```json
{
  "totalTickets": 5,
  "byStatus": {
    "Open": 1,
    "InProgress": 1,
    "Resolved": 1,
    "Closed": 1,
    "Cancelled": 1
  },
  "recentTickets": [ /* last 5 updated, same shape as ticket list */ ]
}
```

---

## Error Response Format

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["Title is required."]
  }
}
```

### Invalid Status Transition

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid status transition",
  "status": 400,
  "detail": "Cannot transition from Open to Resolved. Allowed: InProgress, Cancelled."
}
```

---

## Status Transition Reference

| Current Status | Allowed Next Statuses |
|----------------|----------------------|
| Open | InProgress, Cancelled |
| InProgress | Resolved, Cancelled |
| Resolved | Closed |
| Closed | (none) |
| Cancelled | (none) |
