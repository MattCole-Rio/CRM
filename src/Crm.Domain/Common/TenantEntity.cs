namespace Crm.Domain.Common;

public abstract class TenantEntity : Entity
{
    public Guid TenantId { get; protected set; }

    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected TenantEntity() { }

    protected TenantEntity(Guid tenantId)
    {
        TenantId = tenantId;
    }

    public void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
