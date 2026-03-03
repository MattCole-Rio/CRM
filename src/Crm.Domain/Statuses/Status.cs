using Crm.Domain.Common;

namespace Crm.Domain.Statuses;

public sealed class Status : TenantEntity
{
    private Status() { }

    public Status(Guid tenantId, Guid statusTypeId, string key, string name, int sortOrder, bool isActive = true)
        : base(tenantId)
    {
        StatusTypeId = statusTypeId;
        Key = string.IsNullOrWhiteSpace(key) ? throw new ArgumentException("Key required", nameof(key)) : key.Trim();
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required", nameof(name)) : name.Trim();
        SortOrder = sortOrder;
        IsActive = isActive;
    }

    public Guid StatusTypeId { get; private set; }
    public StatusType StatusType { get; private set; } = null!;

    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }
}