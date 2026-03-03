namespace Crm.Infrastructure.Data.Entities;

public sealed class Permission
{
    public Guid PermissionId { get; set; }
    public Guid TenantId { get; set; }

    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
}