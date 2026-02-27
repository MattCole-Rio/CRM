# Status Model

## Requirement
Statuses must be configurable “in a separate table” and usable for multiple pipelines:
- Business statuses
- Contact statuses (optional)
- Customer statuses (for ProjectParty / lead stage)
- Project, Estimate, HR workflows, etc.

## Recommended schema
### StatusType
- `StatusTypeId` (Guid)
- `TenantId` (Guid)
- `Key` (string, unique per tenant): e.g. `BusinessStatus`, `CustomerStatus`, `ProjectStatus`
- `Name` (string)

### Status
- `StatusId` (Guid)
- `TenantId` (Guid)
- `StatusTypeId` (FK)
- `Key` (string, unique within StatusType): e.g. `Prospect`, `Qualified`
- `Name` (string)
- `SortOrder` (int)
- `IsActive` (bool)

## Guardrails
- Application layer validates that when updating e.g. `Business.BusinessStatusId`, the target `Status.StatusType.Key == "BusinessStatus"`.
- Consider preventing deletion of Status values that are in use (soft-delete / IsActive).

## Seed data
Seed minimal StatusTypes and a few Statuses so the app can run:
- BusinessStatus: Prospect, Active, Dormant
- ContactStatus: Active, Inactive (optional)
- CustomerStatus: PotentialLead, Qualified, Won, Lost (later)
