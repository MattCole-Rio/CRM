namespace Crm.Application.Common;

public interface ITenantProvider
{
    Guid TenantId { get; }
}