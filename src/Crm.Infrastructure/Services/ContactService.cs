using Crm.Application.CRM;
using Crm.Application.CRM.DTOs;
using Crm.Application.Common;
using Crm.Application.RecordAccess;
using Crm.Domain.CRM;
using Crm.Infrastructure.Data;
using Crm.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crm.Infrastructure.Services;

public sealed class ContactService : IContactService
{
    private readonly AppDbContext _db;
    private readonly ITenantProvider _tenant;
    private readonly IRecordAccessService _recordAccess;

    public ContactService(AppDbContext db, ITenantProvider tenant, IRecordAccessService recordAccess)
    {
        _db = db;
        _tenant = tenant;
        _recordAccess = recordAccess;
    }

    public async Task<IReadOnlyList<ContactDto>> GetContactsAsync(string userId, CancellationToken ct = default)
    {
        var query = _db.Contacts.Where(c => c.TenantId == _tenant.TenantId);
        query = (IQueryable<Contact>)_recordAccess.FilterByUser(query, userId);

        return await query
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .Select(c => new ContactDto(c.Id, c.FirstName, c.LastName, c.Email, c.Phone))
            .ToListAsync(ct);
    }

    public async Task<ContactDto?> GetContactAsync(string userId, Guid contactId, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessContactAsync(userId, contactId, ct))
            return null;

        var c = await _db.Contacts.SingleOrDefaultAsync(x => x.TenantId == _tenant.TenantId && x.Id == contactId, ct);
        return c is null ? null : new ContactDto(c.Id, c.FirstName, c.LastName, c.Email, c.Phone);
    }

    public async Task<Guid> CreateAsync(string userId, string firstName, string lastName, string? email, string? phone, CancellationToken ct = default)
    {
        var contact = new Contact(_tenant.TenantId, firstName, lastName);
        contact.SetContactInfo(email, phone);

        _db.Contacts.Add(contact);

        // Auto-assign creator
        _db.ContactAssignments.Add(new ContactAssignment
        {
            TenantId = _tenant.TenantId,
            ContactId = contact.Id,
            UserId = userId,
            AssignmentType = AssignmentType.AccountManager
        });

        await _db.SaveChangesAsync(ct);
        return contact.Id;
    }

    public async Task UpdateAsync(string userId, Guid id, string firstName, string lastName, string? email, string? phone, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessContactAsync(userId, id, ct))
            throw new UnauthorizedAccessException();

        var c = await _db.Contacts.SingleAsync(x => x.TenantId == _tenant.TenantId && x.Id == id, ct);
        c.SetName(firstName, lastName);
        c.SetContactInfo(email, phone);
        await _db.SaveChangesAsync(ct);
    }
}