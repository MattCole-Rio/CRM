# CRM (Walking Skeleton)

## Prereqs
- .NET 8 SDK
- SQL Server (local SQL Express, Developer edition, or Azure SQL)

## Configure
Edit `src/Crm.Web/appsettings.Development.json`:
- `ConnectionStrings:DefaultConnection`
- `Tenant:DefaultTenantId` (any GUID)
- Optional: `Authentication:Mode` = `Local` (default), `Entra`, or `Both`

## Run migrations
From repo root:

```bash
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef migrations add InitialCreate -p src/Crm.Infrastructure -s src/Crm.Web -o Data/Migrations
dotnet ef database update -p src/Crm.Infrastructure -s src/Crm.Web
