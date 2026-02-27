using Crm.Domain.Common;

namespace Crm.Domain.Statuses;

public sealed class StatusType : TenantEntity
{
    private StatusType() { }

    public StatusType(Guid tenantId, string key, string name) : base(tenantId)
    {
        Key = string.IsNullOrWhiteSpace(key) ? throw new ArgumentException("Key required", nameof(key)) : key.Trim();
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required", nameof(name)) : name.Trim();
    }

    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
}
