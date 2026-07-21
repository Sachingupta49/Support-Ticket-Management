# Final AI Usage Summary

**Project:** Support Ticket Management System  
**Tool:** Cursor Agent (Claude)  
**Repository:** https://github.com/Sachingupta49/Support-Ticket-Management  
**Period:** 2026-07-21

## How AI Was Used

| Activity | AI Role | Human Role |
|----------|---------|------------|
| Requirements analysis | Drafted initial structure from assessment brief | Reviewed and approved scope |
| Architecture design | Proposed Clean Architecture layout | Confirmed tech stack choices |
| Backend implementation | Generated entities, handlers, validators, controllers | Verified API behavior via Swagger |
| Database setup | Created migrations and seed data | Confirmed LocalDB connection |
| State machine | Implemented `StatusTransitionValidator` and tests | Validated all AC-14–22 transitions |
| Frontend scaffolding | Built React pages, components, API client | Tested in browser, reported UI issues |
| Testing | Wrote unit and integration tests | Ran `dotnet test`, confirmed 70/70 |
| Debugging | Diagnosed WebApplicationFactory, CORS, npm issues | Executed fixes, restarted servers |
| Documentation | Drafted all assessment markdown files | Reviewed for accuracy |
| UI polish | Redesigned CSS, removed duplicate buttons | Confirmed "all functionalities working" |

## Prompt History

Full per-phase prompts are captured in `ai-prompts/`:

| Phase | File |
|-------|------|
| 1 — Planning | `phase-01-planning.md` |
| 2 — Backend Foundation | `phase-02-backend-foundation.md` |
| 3 — Database | `phase-03-database.md` |
| 4 — Backend Features | `phase-04-backend-features.md` |
| 5 — State Machine | `phase-05-state-machine.md` |
| Backend Tests | `phase-backend-tests.md` |
| 6 — Frontend | `phase-06-frontend.md` |
| 7 — Integration | `phase-07-integration.md` |
| 8 — Testing | `phase-08-testing.md` |
| 9 — Documentation | `phase-09-documentation.md` |

## AI Contribution Estimate

| Category | AI % | Human % |
|----------|------|---------|
| Code generation | ~80% | ~20% (review, direction) |
| Architecture decisions | ~40% | ~60% |
| Testing | ~75% | ~25% (run, verify) |
| Documentation | ~85% | ~15% (review) |
| Debugging | ~60% | ~40% (environment, browser) |

## Effective Patterns

1. **Phase-gated workflow** — "Proceed to Phase N" with confirmation prevented scope creep.
2. **WORKTHROUGH.md tracking** — Gave the agent persistent context across sessions.
3. **Acceptance criteria as checklist** — Mapped tests and manual verification to AC-1–38.
4. **Iterative feedback** — User browser testing caught connection and UI issues AI could not see.

## Limitations Observed

- AI could not detect servers not running or Node.js missing without terminal investigation
- Initial UI had duplicate CTAs — caught by human review, not automated tests
- AI-generated README had duplicate status rows — caught during documentation review

## Conclusion

AI significantly accelerated delivery of a full-stack assessment project. Human oversight remained essential for environment setup, browser verification, and catching UX issues. The prompt history in `ai-prompts/` provides transparency for assessment review.
