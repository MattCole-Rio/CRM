# Domain Model

## CRM core

### Aggregate: Business
**Purpose**: Holds organization details.

Key fields:
- `BusinessId` (Guid)
- `TenantId` (Guid)
- `Name` (required)
- `Abn`/`AcN` (optional)
- `Phone`, `Email`, `Website` (optional)
- `BusinessStatusId` (FK to Status where StatusType = BusinessStatus)

Relationships:
- Many-to-many with Contact via `BusinessContact`
- Assignments via `BusinessAssignment` for record-level security

Invariants:
- Name cannot be empty.
- TenantId required.
- Status must belong to StatusType `BusinessStatus`.

### Aggregate: Contact
**Purpose**: Person record.

Key fields:
- `ContactId` (Guid)
- `TenantId` (Guid)
- `FirstName` (required)
- `LastName` (required)
- `Email` (optional but recommended unique per tenant if used for external users)
- `Phone` (optional)
- `ContactStatusId` (FK to Status where StatusType = ContactStatus) (optional initially)

Relationships:
- Many-to-many with Business via `BusinessContact`
- Local external Identity users must be linked to ≥1 Contact (enforced in app layer + DB constraints where feasible)

Invariants:
- FirstName/LastName not empty.
- If Email present, normalize and validate format in application layer.

### Association: BusinessContact
- `(BusinessId, ContactId)` unique composite key
- Optional fields: `RelationshipType` (e.g., Billing, Primary, Technical) (later)

## Identity & Access (conceptual)
### User
- Backed by ASP.NET Core Identity `ApplicationUser`
- Flags:
  - `UserType`: Staff (Entra) vs External (Local)
  - `TenantId` (Guid)

### UserProfile
- `DepartmentId` (later)
- `ManagerUserId` (for approvals and “manager can see reports”)

### RBAC Entities
- `Permission` (string key)
- `RolePermission`
- `UserPermissionOverride` (Allow/Deny)

## Record-level access model (initial)
- For each secured entity type, create explicit assignment tables:
  - `BusinessAssignment(BusinessId, UserId, AssignmentType)`
  - `ContactAssignment(ContactId, UserId, AssignmentType)`
- Local external users: must have assignments to the Contact(s) they represent.
- Staff access:
  - Direct assignment grants record access.
  - Manager access: access records assigned to users in their reporting line (computed in app layer).

## Status model
- `StatusType` defines a pipeline category.
- `Status` belongs to a StatusType.
- Entities reference a Status (FK) and app ensures it matches the correct StatusType.

## Later modules (preview only)
- Projects: `Project`, `ProjectParty` (BusinessId? ContactId? CustomerStatusId)
- Marketing: `Campaign`, `Unsubscribe`, `EmailProviderConfig`
- HR: `Timesheet`, `TimesheetEntry`, `LeaveRequest`, approval workflow
- Products: `Product` basic catalog + CSV adapters
