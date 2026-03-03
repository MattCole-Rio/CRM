using Crm.Application.Permissions;

namespace Crm.Infrastructure.Data.Entities;

public sealed class UserPermissionOverride
{
    public Guid UserPermissionOverrideId { get; set; }
    public Guid TenantId { get; set; }

    public string UserId { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;

    public PermissionEffect Effect { get; set; }
}