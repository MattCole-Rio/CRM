# Reckon Accounts Premier CSV Integration

## Constraint
No sample files available; only “default templates” from Reckon Accounts Premier (latest).

## Strategy
Implement CSV import/export as:
- Strict parsers for known/default column sets **plus**
- A configurable mapping system if column headers differ

## Recommended approach
### Step 1 (MVP)
- Implement CSV utilities:
  - header detection
  - required column validation
  - row-level error reporting and import logs
- Implement admin-only endpoints/pages:
  - Upload CSV file
  - Preview parsed rows
  - Confirm import
  - Export to CSV (download)

### Step 2 (when defaults mismatch)
- Add “mapping profile” UI:
  - For each integration type (LeaveAccrualImport, TimesheetExport, ProductImport/Export)
  - Map CSV header -> internal field
  - Save profiles per tenant

## Admin-only enforcement
- Feature permission `Integrations.Reckon.Manage` and Admin role requirement.

## Timesheet export
- Export as daily rows per user with:
  - Date
  - Employee identifier (must decide: email vs employeeId)
  - Project/Job code (later)
  - Hours
If Reckon requires specific columns, adjust mapping profile.

## Leave accrual import
- Import leave balances/accrual per employee:
  - employee identifier
  - leave type
  - balance/accrual amount
Without sample: implement mapping profiles and an import log.

## Products CSV
- Basic product fields (SKU, name, price, GST category)
- Import/export mapping profile to match Reckon expectations.
