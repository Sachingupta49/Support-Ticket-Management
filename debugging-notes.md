# Debugging Notes

## Session: 2026-07-21

### Issue: WebApplicationFactory host build failure

**Symptom:** Integration tests failed with "The entry point exited without ever building an IHost."

**Cause:** `Program.cs` wrapped `WebApplication.Build()` in a try-catch-finally block that interfered with the test host factory.

**Fix:** Simplified `Program.cs` to standard minimal hosting pattern with `public partial class Program`.

---

### Issue: Integration test database seeding in ConfigureServices

**Symptom:** `EnsureCreated()` called during service configuration caused DI resolution errors.

**Fix:** Moved seeding to `CustomWebApplicationFactory.InitializeAsync()` after host is built.

---

### Issue: Shared in-memory database state across tests

**Symptom:** State transition tests mutated shared ticket IDs.

**Fix:** Each transition test creates its own ticket; invalid-transition tests use seeded terminal-state tickets.

---

### Issue: API file lock during test run

**Symptom:** `MSB3027` build errors — DLL locked by running `SupportTicket.API` process.

**Fix:** Stop the API process before running `dotnet test`.

---

### Issue: npm not in PATH

**Symptom:** `npm create vite` failed in automated environment.

**Fix:** Frontend scaffolded manually with all source files; user runs `npm install` locally.

---

### Browser Testing Notes

- API runs on `http://localhost:5172` (launch profile `http`)
- Frontend runs on `http://localhost:5173` with Vite proxy to API
- Swagger available at `http://localhost:5172/swagger`
- CORS configured for `http://localhost:5173`
