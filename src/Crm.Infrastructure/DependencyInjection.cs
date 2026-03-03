using Crm.Application.CRM;
using Crm.Application.Common;
using Crm.Application.Permissions;
using Crm.Application.RecordAccess;
using Crm.Infrastructure.Data;
using Crm.Infrastructure.Identity;
using Crm.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRecordAccessService, RecordAccessService>();

        services.AddScoped<IBusinessService, BusinessService>();
        services.AddScoped<IContactService, ContactService>();

        // Tenant provider (single-tenant now)
        var tenantId = Guid.Parse(config["Tenant:DefaultTenantId"] ?? throw new InvalidOperationException("Tenant:DefaultTenantId missing"));
        services.AddSingleton<ITenantProvider>(new TenantProvider(tenantId));

        // For PermissionService user manager generic mismatch, register IdentityUser manager mapping:
        services.AddScoped<UserManager<IdentityUser>>(sp => (UserManager<IdentityUser>)(object)sp.GetRequiredService<UserManager<ApplicationUser>>());

        return services;
    }
}