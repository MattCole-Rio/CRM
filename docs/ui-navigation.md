# UI Navigation (Blazor Server)

## Navigation groups
- Dashboard (later)
- CRM
  - Businesses
  - Contacts
- Projects (later)
- Marketing (later)
- HR (later)
- Admin
  - Roles & Permissions
  - Status Configuration
  - User Management (basic)
  - Email Configuration (later)
  - Integrations (Reckon CSV) (later)

## Page access control
- Use `AuthorizeView` and route-level `[Authorize]` policies.
- Every CRUD action also calls application-layer authorization checks.

## Business pages (MVP)
- Business list (filtered by record-level access)
- Business details
  - edit business
  - linked contacts (add/remove link)
  - assignments (account manager) (optional in MVP)

## Contact pages (MVP)
- Contact list (filtered)
- Contact details
  - edit contact
  - linked businesses
