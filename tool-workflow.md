# Tool Workflow

## AI Tools Used

| Tool | Role |
|------|------|
| Cursor IDE | Primary development environment |
| Cursor Agent | Code generation, planning, documentation |
| Multi-agent coordination | Parallel workstreams per execution plan |

## Agent Roles (per Execution Plan)

| Agent | Responsibility |
|-------|----------------|
| Coordinator | Progress tracking, Git management, phase confirmation |
| Backend Agent | .NET API, EF Core, business logic |
| Frontend Agent | React UI, API integration |
| Database Agent | Schema, migrations, seed data |
| QA Agent | Unit and integration tests |
| Documentation Agent | Assessment markdown files |
| AI Workflow Agent | Prompt history, reflection notes |

## Workflow Rules

1. **Sequential phases** — complete and confirm each phase before proceeding
2. **Commit per phase** — one meaningful commit per completed phase
3. **Work-through tracking** — update `WORKTHROUGH.md` after each phase
4. **Prompt logging** — save prompts to `ai-prompts/` directory
5. **No autonomous phase skipping** — user confirms before next step

## Phase 1 Prompt (Initial)

**User request:**
> Read the Support Ticket Assessment Execution Plan and start creating step-by-step. Do not move to the next step autonomously. Get confirmation after completing each step. Create a work-through MD file with each step's information and commit history pushed to a git repo.

**Approach:**
- Read execution plan from Downloads
- Initialize empty workspace with git
- Create all Phase 1 planning deliverables
- Document progress in `WORKTHROUGH.md`
- Commit as "Initial Planning"
- Await user confirmation before Phase 2

## Development Conventions

- Follow existing code patterns in each layer
- Minimal scope per commit
- No secrets in repository
- Tests added when behavior changes
- Documentation updated alongside code
