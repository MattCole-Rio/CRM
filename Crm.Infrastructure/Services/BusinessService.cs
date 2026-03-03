using Crm.Application.CRM;
using Crm.Application.CRM.DTOs;
using Crm.Application.Common;
using Crm.Application.RecordAccess;
using Crm.Domain.CRM;
using Crm.Infrastructure.Data;
using Crm.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crm.Infrastructure.Services;

public sealed class BusinessService : IBusinessService
{
    private readonly AppDbContext _db;
    private readonly ITenantProvider _tenant;
    private readonly IRecordAccessService _recordAccess;

    public BusinessService(AppDbContext db, ITenantProvider tenant, IRecordAccessService recordAccess)
    {
        _db = db;
        _tenant = tenant;
        _recordAccess = recordAccess;
    }

    public async Task<IReadOnlyList<BusinessDto>> GetBusinessesAsync(string userId, CancellationToken ct = default)
    {
        var query = _db.Businesses.Where(b => b.TenantId == _tenant.TenantId);
        query = (IQueryable<Business>)_recordAccess.FilterByUser(query, userId);

        return await query
            .OrderBy(b => b.Name)
            .Select(b => new BusinessDto(b.Id, b.Name, b.Email, b.Phone, b.Website))
            .ToListAsync(ct);
    }

    public async Task<BusinessDto?> GetBusinessAsync(string userId, Guid businessId, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessBusinessAsync(userId, businessId, ct))
            return null;

        var b = await _db.Businesses.SingleOrDefaultAsync(x => x.TenantId == _tenant.TenantId && x.Id == businessId, ct);
        return b is null ? null : new BusinessDto(b.Id, b.Name, b.Email, b.Phone, b.Website);
    }

    public async Task<Guid> CreateAsync(string userId, string name, string? email, string? phone, string? website, CancellationToken ct = default)
    {
        var business = new Business(_tenant.TenantId, name);
        business.SetContactInfo(email, phone, website);

        _db.Businesses.Add(business);

        // Auto-assign creator for record access
        _db.BusinessAssignments.Add(new BusinessAssignment
        {
            TenantId = _tenant.TenantId,
            BusinessId = business.Id,
            UserId = userId,
            AssignmentType = AssignmentType.AccountManager
        });

        await _db.SaveChangesAsync(ct);
        return business.Id;
    }

    public async Task UpdateAsync(string userId, Guid id, string name, string? email, string? phone, string? website, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessBusinessAsync(userId, id, ct))
            throw new UnauthorizedAccessException();

        var b = await _db.Businesses.SingleAsync(x => x.TenantId == _tenant.TenantId && x.Id == id, ct);
        b.SetName(name);
        b.SetContactInfo(email, phone, website);
        await _db.SaveChangesAsync(ct);
    }

    public async Task LinkContactAsync(string userId, Guid businessId, Guid contactId, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessBusinessAsync(userId, businessId, ct))
            throw new UnauthorizedAccessException();

        if (!await _recordAccess.CanAccessContactAsync(userId, contactId, ct))
            throw new UnauthorizedAccessException();

        var exists = await _db.BusinessContacts.AnyAsync(x => x.BusinessId == businessId && x.ContactId == contactId, ct);
        if (exists) return;

        _db.BusinessContacts.Add(new BusinessContact
        {
            TenantId = _tenant.TenantId,
            BusinessId = businessId,
            ContactId = contactId
        });

        await _db.SaveChangesAsync(ct);
    }

    public async Task UnlinkContactAsync(string userId, Guid businessId, Guid contactId, CancellationToken ct = default)
    {
        if (!await _recordAccess.CanAccessBusinessAsync(userId, businessId, ct))
            throw new UnauthorizedAccessException();

        var link = await _db.BusinessContacts.SingleOrDefaultAsync(x => x.BusinessId == businessId && x.ContactId == contactId, ct);
        if (link is null) return;

        _db.BusinessContacts.Remove(link);
        await _db.SaveChangesAsync(ct);
    }
}