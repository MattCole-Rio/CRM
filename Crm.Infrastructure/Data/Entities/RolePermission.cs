namespace Crm.Infrastructure.Data.Entities;

public sealed class RolePermission
{
    public Guid TenantId { get; set; }

    public string RoleId { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}