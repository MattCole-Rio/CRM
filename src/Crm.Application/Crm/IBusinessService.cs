using Crm.Application.CRM.DTOs;

namespace Crm.Application.CRM;

public interface IBusinessService
{
    Task<IReadOnlyList<BusinessDto>> GetBusinessesAsync(string userId, CancellationToken ct = default);
    Task<BusinessDto?> GetBusinessAsync(string userId, Guid businessId, CancellationToken ct = default);
    Task<Guid> CreateAsync(string userId, string name, string? email, string? phone, string? website, CancellationToken ct = default);
    Task UpdateAsync(string userId, Guid id, string name, string? email, string? phone, string? website, CancellationToken ct = default);

    Task LinkContactAsync(string userId, Guid businessId, Guid contactId, CancellationToken ct = default);
    Task UnlinkContactAsync(string userId, Guid businessId, Guid contactId, CancellationToken ct = default);
}