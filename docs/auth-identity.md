# Authentication & Identity

## Staff users (Entra ID)
- Use OpenID Connect via `Microsoft.Identity.Web`
- Single-tenant now:
  - Configure `AzureAd:TenantId`, `ClientId`, `ClientSecret` (or certificate)
- Map Entra user to local app user record:
  - Store `EntraObjectId` (GUID) on `AspNetUsers`
  - On first login, create/update user record and mark `UserType=Staff`

## External users (Local Identity)
- Use ASP.NET Core Identity local accounts
- `UserType=External`
- Must be linked to at least one Contact:
  - Approach:
    - External user created with `ExternalContactId` (preferred)
    - OR invited first, then admin links to contact later (must block access to CRM data until linked)

## Authorization policies
- `RequireStaffUser` (UserType=Staff)
- `RequireExternalUser` (UserType=External)
- `RequireAdminRole` (role name Admin)
- Feature permission policies: `RequirePermission("CRM.Business.View")`, etc.

## External user restrictions
- Only access:
  - their own profile
  - contacts they are linked to
  - businesses/projects/tasks they are assigned to via assignment tables

Implementation notes:
- Always enforce record-level checks in Application layer services, not only in UI.
- UI uses authorization to hide nav items, but backend checks are authoritative.
