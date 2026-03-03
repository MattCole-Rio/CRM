using Crm.Application.Permissions;
using Crm.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Infrastructure.Services;

public sealed class PermissionService : IPermissionService
{
    private readonly AppDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public PermissionService(AppDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permissionKey, CancellationToken ct = default)
    {
        var user = await _db.Users.SingleAsync(u => u.Id == userId, ct);

        // Get permission id
        var permission = await _db.Permissions.SingleOrDefaultAsync(p => p.TenantId == user.TenantId && p.Key == permissionKey, ct);
        if (permission is null)
            return false;

        // Overrides take precedence
        var overrideRow = await _db.UserPermissionOverrides
            .SingleOrDefaultAsync(o => o.TenantId == user.TenantId && o.UserId == userId && o.PermissionId == permission.PermissionId, ct);

        if (overrideRow?.Effect == PermissionEffect.Deny)
            return false;
        if (overrideRow?.Effect == PermissionEffect.Allow)
            return true;

        // Role permissions
        var roleIds = await _db.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync(ct);

        var hasRolePerm = await _db.RolePermissions.AnyAsync(rp =>
            rp.TenantId == user.TenantId &&
            roleIds.Contains(rp.RoleId) &&
            rp.PermissionId == permission.PermissionId, ct);

        return hasRolePerm;
    }
}