using Crm.Application.Common;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Crm.Web.Services;

public sealed class CurrentUser : ICurrentUser
{
    private readonly AuthenticationStateProvider _auth;

    public CurrentUser(AuthenticationStateProvider auth)
    {
        _auth = auth;
    }

    public string? UserId
    {
        get
        {
            var user = GetUser().GetAwaiter().GetResult();
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

    public string? Email
    {
        get
        {
            var user = GetUser().GetAwaiter().GetResult();
            return user.FindFirstValue(ClaimTypes.Email) ?? user.Identity?.Name;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            var user = GetUser().GetAwaiter().GetResult();
            return user.Identity?.IsAuthenticated == true;
        }
    }

    public bool IsInRole(string role)
    {
        var user = GetUser().GetAwaiter().GetResult();
        return user.IsInRole(role);
    }

    private async Task<ClaimsPrincipal> GetUser()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        return state.User;
    }
}