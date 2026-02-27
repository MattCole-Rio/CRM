# RBAC & Permissions

## Roles
Initial roles (configurable later):
- Sales Member
- Sales Manager
- Marketing Member
- Marketing Manager
- Admin

## Permission model
### Feature permissions
Represent permissions as string keys:
- `CRM.Business.View`
- `CRM.Business.Create`
- `CRM.Business.Edit`
- `CRM.Contact.View`
- `CRM.Contact.Create`
- `CRM.Contact.Edit`
- `RBAC.Manage` (admin UI)
- etc.

### Sources of permission
Effective permission is resolved from:
1. Role permissions (many-to-many)
2. User overrides (Allow/Deny) take precedence

Resolution rules:
- If any override = Deny => denied
- Else if any override = Allow => allowed
- Else allowed if any assigned role has permission
- Else denied

## Record-level access
### What is “record-level”
Even if a user has `CRM.Business.View`, they only see businesses they are allowed to access.

### Initial enforcement model (assignment-based)
- `BusinessAssignments` and `ContactAssignments` tables drive access
- `Admin` bypasses record-level restrictions
- Staff:
  - Can access records assigned to themselves
  - Managers additionally can access records assigned to users in their reporting tree (ManagerUserId)
- External (local) users:
  - Can access only records assigned to themselves (and/or linked via ExternalContactId)

### How to implement checks
- Provide an `IAuthorizationService`-like app service:
  - `CanViewBusiness(user, businessId)`
  - `FilterBusinessesQueryable(user, query)`
- Implement with EF Core query composition:
  - If Admin => return query
  - Else join assignments with current user and (if manager) users in report tree

## Admin UI for permissions
Minimum UI (MVP):
- List roles
- View permissions per role
- Toggle permissions for a role
- List user overrides
- Add override Allow/Deny for a user

Record-level assignment UI (MVP):
- Assign “Account Manager” (or Contributor) to Business/Contact for a user
