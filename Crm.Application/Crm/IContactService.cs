using Crm.Application.CRM.DTOs;

namespace Crm.Application.CRM;

public interface IContactService
{
    Task<IReadOnlyList<ContactDto>> GetContactsAsync(string userId, CancellationToken ct = default);
    Task<ContactDto?> GetContactAsync(string userId, Guid contactId, CancellationToken ct = default);
    Task<Guid> CreateAsync(string userId, string firstName, string lastName, string? email, string? phone, CancellationToken ct = default);
    Task UpdateAsync(string userId, Guid id, string firstName, string lastName, string? email, string? phone, CancellationToken ct = default);
}