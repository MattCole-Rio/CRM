using Crm.Application.Common;

namespace Crm.Infrastructure.Services;

public sealed class TenantProvider : ITenantProvider
{
    public Guid TenantId { get; }

    public TenantProvider(Guid tenantId)
    {
        TenantId = tenantId;
    }
}