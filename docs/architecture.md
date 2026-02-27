# Architecture

## Goals
- Internal LoB CRM built with **.NET 8** and **Blazor Server**
- **SQL Server** persistence via **EF Core**
- Authentication: **Entra ID SSO** for staff + **local accounts** for external users
- Authorization: configurable **RBAC** with **role permissions** + **user overrides** and **record-level access**
- DDD-inspired layering to keep business logic testable and isolated

## Deployment targets
- Azure-first (App Service or container)
- On-prem compatible (IIS/Windows Server or container)
- Avoid Azure-only dependencies in core logic; keep them in Infrastructure

## Solution layout (recommended)
- `Crm.Domain`
  - Aggregates, entities, value objects, domain services (pure logic)
  - Domain events (optional later)
- `Crm.Application`
  - Use cases (commands/queries), DTOs, validation, authorization checks
  - Interfaces for persistence and integrations
- `Crm.Infrastructure`
  - EF Core DbContext, migrations
  - Identity stores, Graph/SMTP email, CSV import/export adapters
- `Crm.Web`
  - Blazor Server UI (pages/components)
  - Authentication configuration, DI composition root
- `Crm.Tests.Unit`
  - Domain + Application unit tests (fast)
- `Crm.Tests.Integration` (optional)
  - EF Core integration tests with SQL Server (Testcontainers)

## Modularity (bounded contexts / modules)
For a modular monolith, implement as namespaces/projects that map to bounded contexts:

1. Identity & Access
2. CRM (Business/Contact)
3. Projects & Estimates (later)
4. Marketing (later)
5. HR (later)
6. Products (later)
7. Integrations (Graph/SMTP/Reckon) (later, but stubs now)

## Data access strategy
- EF Core for operational data
- Use `DbContext` per application (single DbContext initially is OK)
- Keep domain invariants in Domain layer; EF entities may map 1:1 but avoid putting persistence concerns in domain methods

## Multi-tenancy strategy (future-proofing)
- Include `TenantId` (GUID) column on all business data tables
- Single tenant now: resolved from config
- Future: tenant resolution from host/header/Entra tenant mapping

## Cross-cutting concerns
- Auditing: `CreatedAtUtc`, `UpdatedAtUtc`, `CreatedByUserId`, `UpdatedByUserId` (later)
- Soft delete: optional (defer)
- Logging: standard `ILogger`
- Validation: FluentValidation or DataAnnotations (choose one; FluentValidation recommended)
