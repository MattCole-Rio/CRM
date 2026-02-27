# Database Schema (Outline)

All tables include:
- `TenantId` UNIQUEIDENTIFIER NOT NULL
- Auditing: `CreatedAtUtc` DATETIME2 NOT NULL, `UpdatedAtUtc` DATETIME2 NULL

## Identity (ASP.NET Core Identity)
Use standard Identity tables plus:
- `AspNetUsers` extended with:
  - `TenantId` UNIQUEIDENTIFIER
  - `UserType` (int or string) e.g. Staff/External
  - `ExternalContactId` UNIQUEIDENTIFIER NULL (for external users linkage)
  - `EntraObjectId` UNIQUEIDENTIFIER NULL (for staff mapping)

## CRM tables
### Businesses
- `BusinessId` UNIQUEIDENTIFIER PK
- `TenantId`
- `Name` NVARCHAR(200) NOT NULL
- `Email` NVARCHAR(256) NULL
- `Phone` NVARCHAR(50) NULL
- `Website` NVARCHAR(256) NULL
- `BusinessStatusId` UNIQUEIDENTIFIER NULL FK -> Statuses(StatusId)

Indexes:
- `IX_Businesses_TenantId_Name`
- optional unique: `TenantId + Name` (consider collisions; might not be unique)

### Contacts
- `ContactId` UNIQUEIDENTIFIER PK
- `TenantId`
- `FirstName` NVARCHAR(100) NOT NULL
- `LastName` NVARCHAR(100) NOT NULL
- `Email` NVARCHAR(256) NULL
- `Phone` NVARCHAR(50) NULL
- `ContactStatusId` UNIQUEIDENTIFIER NULL FK -> Statuses

Indexes:
- `IX_Contacts_TenantId_LastName_FirstName`
- optional unique filtered index: `TenantId + NormalizedEmail` where Email is not null

### BusinessContacts (many-to-many)
- `TenantId`
- `BusinessId` FK -> Businesses
- `ContactId` FK -> Contacts
- PK: `(BusinessId, ContactId)`

## Status configuration
### StatusTypes
- `StatusTypeId` PK
- `TenantId`
- `Key` NVARCHAR(100) NOT NULL
- `Name` NVARCHAR(200) NOT NULL
- Unique: `(TenantId, Key)`

### Statuses
- `StatusId` PK
- `TenantId`
- `StatusTypeId` FK -> StatusTypes
- `Key` NVARCHAR(100) NOT NULL
- `Name` NVARCHAR(200) NOT NULL
- `SortOrder` INT NOT NULL
- `IsActive` BIT NOT NULL
- Unique: `(TenantId, StatusTypeId, Key)`

## RBAC tables
### Permissions
- `PermissionId` UNIQUEIDENTIFIER PK
- `TenantId`
- `Key` NVARCHAR(200) NOT NULL
- `Description` NVARCHAR(500) NULL
- Unique: `(TenantId, Key)`

### RolePermissions
- `TenantId`
- `RoleId` (from AspNetRoles)
- `PermissionId`
- PK: `(RoleId, PermissionId)`

### UserPermissionOverrides
- `UserPermissionOverrideId` PK
- `TenantId`
- `UserId` (AspNetUsers)
- `PermissionId`
- `Effect` INT NOT NULL (Allow=1, Deny=2)
- Unique: `(TenantId, UserId, PermissionId)`

## Record-level assignment tables (initial)
### BusinessAssignments
- `TenantId`
- `BusinessId`
- `UserId`
- `AssignmentType` INT (AccountManager=1, Contributor=2, Viewer=3)
- PK: `(BusinessId, UserId)`

### ContactAssignments
- `TenantId`
- `ContactId`
- `UserId`
- `AssignmentType` INT
- PK: `(ContactId, UserId)`

## Notes
- Enforce TenantId consistency in application layer (and optionally via DB triggers later).
- For multi-tenant later, add global query filters in EF Core based on ITenantProvider.
