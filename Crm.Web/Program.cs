using Crm.Application.Permissions;
using Crm.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

// Infrastructure (Db, Identity, services)
builder.Services.AddInfrastructure(builder.Configuration);

// Auth mode toggle
var authMode = builder.Configuration["Authentication:Mode"]?.Trim() ?? "Local";

if (authMode.Equals("Entra", StringComparison.OrdinalIgnoreCase) || authMode.Equals("Both", StringComparison.OrdinalIgnoreCase))
{
    builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
        .AddInMemoryTokenCaches();

    builder.Services.AddControllersWithViews()
        .AddMicrosoftIdentityUI();
}
else
{
    // Local Identity cookie auth is already registered by AddIdentity in Infrastructure.
    builder.Services.AddControllersWithViews();
}

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequirePermission_RbacManage", policy =>
        policy.RequireAssertion(_ => true)); // placeholder; enforced in pages via PermissionService
});

// Current user abstraction
builder.Services.AddScoped<Crm.Application.Common.ICurrentUser, Crm.Web.Services.CurrentUser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();