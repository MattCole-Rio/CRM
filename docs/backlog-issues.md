# Ready-to-paste GitHub Issue Backlog

Paste each section as a GitHub issue. Use `Epic:` titles for epics.

---

## Epic: Repository & Solution Setup (.NET 8)
**Description**
Set up solution structure, projects, basic docs, and baseline CI-ready build.

**Acceptance Criteria**
- Solution builds with `dotnet build`
- Unit tests project runs with `dotnet test`
- `/docs` added with initial design pack

**Tasks**
- Create solution + projects (Domain/Application/Infrastructure/Web/Tests)
- Add EditorConfig, analyzers (optional)
- Add README and docs

---

## Epic: Authentication (Entra ID + Local Identity)
**Description**
Enable staff SSO with Entra and external local accounts.

**Acceptance Criteria**
- Staff can sign in via Entra in dev configuration
- Local users can register/invite and sign in
- App can distinguish Staff vs External user types

**Stories**
1. Staff login via Microsoft.Identity.Web (single-tenant)
2. Local Identity setup with ApplicationUser extensions
3. Provision user record on first Entra login
4. External user must be linked to a Contact before accessing CRM data

---

## Epic: RBAC (Feature permissions + User overrides)
**Description**
Implement permission entities and evaluation logic.

**Acceptance Criteria**
- Permissions defined as keys (string)
- Role permissions grant access
- User overrides allow/deny take precedence
- Unit tests cover resolution rules

**Stories**
1. Data model: Permission, RolePermission, UserPermissionOverride
2. Permission evaluation service + tests
3. Seed default roles + base permissions

---

## Epic: Record-level access (Assignments + Manager tree)
**Description**
Implement assignment-based record access enforcement.

**Acceptance Criteria**
- Admin bypass
- Staff can only see assigned businesses/contacts
- Managers can see records assigned to their reports
- External users can only see records linked/assigned to them
- Covered by tests (at least for filtering logic)

**Stories**
1. Assignment tables for Business and Contact
2. UserProfile with ManagerUserId
3. Query filters in application services

---

## Epic: Status configuration
**Description**
Configurable statuses by StatusType.

**Acceptance Criteria**
- StatusType and Status tables exist
- Seed minimal default statuses
- Admin UI to manage StatusTypes/Statuses (basic)

**Stories**
1. EF model + migrations + seed
2. Admin UI for statuses

---

## Epic: CRM - Businesses
**Description**
CRUD for Businesses, secured by permissions and record-level access.

**Acceptance Criteria**
- Business list/detail/create/edit pages in Blazor
- Permission keys enforced (View/Create/Edit)
- Record-level filtering works

**Stories**
1. Business domain model + persistence
2. Business CRUD UI
3. Business assignments UI (optional MVP)

---

## Epic: CRM - Contacts
**Description**
CRUD for Contacts, secured by permissions and record-level access.

**Acceptance Criteria**
- Contact list/detail/create/edit pages
- Permission enforcement
- Record-level filtering works

**Stories**
1. Contact domain model + persistence
2. Contact CRUD UI
3. Contact assignments UI (optional MVP)

---

## Epic: CRM - Business/Contact linking
**Description**
Many-to-many linking management.

**Acceptance Criteria**
- Link/unlink contacts to businesses
- Visible on business detail page
- Enforced by permissions

**Stories**
1. BusinessContact persistence + UI component

---

## Epic: Admin UI - Roles/Permissions management
**Description**
Minimal UI for managing permissions per role and user overrides.

**Acceptance Criteria**
- Admin can view roles
- Admin can view and edit role permission assignments
- Admin can add user overrides Allow/Deny

**Stories**
1. Roles & permission assignment page
2. User override management page

---

## Epic: Docs & local dev
**Description**
Document running locally and configuring auth.

**Acceptance Criteria**
- Docs explain SQL connection string + migrations
- Docs explain Entra config
- Docs explain permission model and seeded permissions

**Stories**
1. Write `docs/running-locally.md` (or expand existing docs)
2. Write `docs/entra-setup.md`
3. Write `docs/rbac-howto.md`

---

## Epic: Integrations (Reckon CSV) - Foundations (Admin-only)
**Description**
Create the plumbing for import/export with logs and mapping profiles (optional).

**Acceptance Criteria**
- Admin-only access
- Upload/parse CSV with error reporting
- Export CSV skeleton for timesheets/products (placeholder)

**Stories**
1. CSV parsing utilities + import log model
2. Admin UI stub for integration runs
3. Mapping profiles (if needed)
