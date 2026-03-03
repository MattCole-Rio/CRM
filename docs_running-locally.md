# Running locally

1. Set connection string in `src/Crm.Web/appsettings.Development.json`
2. Create migrations and update database:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate -p src/Crm.Infrastructure -s src/Crm.Web -o Data/Migrations
dotnet ef database update -p src/Crm.Infrastructure -s src/Crm.Web