# Testing Strategy

## Unit tests (required)
- Domain tests:
  - Business/Contact invariants (non-empty names)
  - Status assignment validation (via application service or domain service)
- Application tests:
  - Permission resolution (role permissions + user overrides)
  - Record-level filtering logic (assignment-based) using in-memory queryable or EF Core InMemory

Tools:
- xUnit
- FluentAssertions

## Integration tests (recommended)
Use Testcontainers for SQL Server to validate:
- EF migrations apply cleanly
- Record-level filtering queries work with SQL translation
- Identity tables created correctly

## End-to-end smoke test (optional)
- Minimal Playwright test for login is hard with Entra; defer.
- Local Identity login smoke test is feasible.

## Definition of Done
- `dotnet build` succeeds
- `dotnet test` succeeds
- Migrations generated and documented
- Key authorization paths covered by tests
