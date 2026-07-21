# Data Model

## Entity Relationship Diagram

```text
┌──────────────┐       ┌──────────────────┐       ┌──────────────┐
│    User      │       │     Ticket       │       │   Comment    │
├──────────────┤       ├──────────────────┤       ├──────────────┤
│ Id (PK)      │◄──┐   │ Id (PK)          │◄──────│ Id (PK)      │
│ Name         │   │   │ Title            │       │ TicketId(FK) │
│ Email        │   ├───│ AssigneeId (FK)  │       │ AuthorId(FK) │
│ CreatedAt    │   │   │ Status           │       │ Body         │
└──────────────┘   │   │ Priority         │       │ CreatedAt    │
       ▲           │   │ Description      │       └──────────────┘
       │           │   │ CreatedAt        │              ▲
       └───────────┘   │ UpdatedAt        │              │
                       └──────────────────┘              │
                              ▲                          │
                              └──────────────────────────┘
```

## Entities

### User

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, Identity |
| Name | nvarchar(100) | Required |
| Email | nvarchar(256) | Required, Unique |
| CreatedAt | datetime2 | Required, default UTC now |

### Ticket

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, Identity |
| Title | nvarchar(200) | Required |
| Description | nvarchar(max) | Required |
| Status | int (enum) | Required, default Open |
| Priority | int (enum) | Required, default Medium |
| AssigneeId | int | FK → User, Required |
| CreatedAt | datetime2 | Required |
| UpdatedAt | datetime2 | Required |

### Comment

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, Identity |
| TicketId | int | FK → Ticket, Required, Cascade delete |
| AuthorId | int | FK → User, Required |
| Body | nvarchar(2000) | Required |
| CreatedAt | datetime2 | Required |

## Enums

### TicketStatus

| Value | Name | Description |
|-------|------|-------------|
| 0 | Open | Newly created ticket |
| 1 | InProgress | Being worked on |
| 2 | Resolved | Fix applied, awaiting closure |
| 3 | Closed | Completed and closed |
| 4 | Cancelled | Cancelled without resolution |

### TicketPriority

| Value | Name |
|-------|------|
| 0 | Low |
| 1 | Medium |
| 2 | High |
| 3 | Critical |

## State Transition Matrix

| From \ To | InProgress | Resolved | Closed | Cancelled |
|-----------|:----------:|:--------:|:------:|:---------:|
| Open | ✅ | ❌ | ❌ | ✅ |
| InProgress | ❌ | ✅ | ❌ | ✅ |
| Resolved | ❌ | ❌ | ✅ | ❌ |
| Closed | ❌ | ❌ | ❌ | ❌ |
| Cancelled | ❌ | ❌ | ❌ | ❌ |

## Seed Data

### Users (minimum 3)

| Name | Email |
|------|-------|
| Alice Agent | alice@support.com |
| Bob Agent | bob@support.com |
| Carol Manager | carol@support.com |

### Sample Tickets (minimum 5)

| Title | Status | Priority | Assignee |
|-------|--------|----------|----------|
| Cannot login to portal | Open | High | Alice |
| Printer not working | InProgress | Medium | Bob |
| Email sync delay | Resolved | Low | Alice |
| VPN connection drops | Closed | High | Carol |
| Request new monitor | Cancelled | Low | Bob |

## Indexes

| Table | Index | Purpose |
|-------|-------|---------|
| Ticket | IX_Ticket_Status | Status filter queries |
| Ticket | IX_Ticket_AssigneeId | Assignee lookups |
| Comment | IX_Comment_TicketId | Comment list by ticket |
| User | IX_User_Email (unique) | Email uniqueness |

## EF Core Configuration Notes

- `Ticket.Status` and `Ticket.Priority` stored as integers
- `Comment` cascade delete when parent ticket is deleted
- `UpdatedAt` set in application layer on every ticket modification
- Navigation properties: `Ticket.Assignee`, `Ticket.Comments`, `Comment.Author`, `Comment.Ticket`
