using Crm.Application.RecordAccess;
using Crm.Domain.CRM;
using Crm.Infrastructure.Data;
using Crm.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Infrastructure.Services;

public sealed class RecordAccessService : IRecordAccessService
{
    private readonly AppDbContext _db;

    public RecordAccessService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> CanAccessBusinessAsync(string userId, Guid businessId, CancellationToken ct = default)
    {
        var user = await _db.Users.SingleAsync(u => u.Id == userId, ct);

        if (await IsAdminAsync(userId, ct))
            return true;

        return await _db.BusinessAssignments.AnyAsync(a =>
            a.TenantId == user.TenantId && a.BusinessId == businessId && a.UserId == userId, ct);
    }

    public async Task<bool> CanAccessContactAsync(string userId, Guid contactId, CancellationToken ct = default)
    {
        var user = await _db.Users.SingleAsync(u => u.Id == userId, ct);

        if (await IsAdminAsync(userId, ct))
            return true;

        // External users can also access their linked contact.
        if (user.UserType == UserType.External && user.ExternalContactId == contactId)
            return true;

        return await _db.ContactAssignments.AnyAsync(a =>
            a.TenantId == user.TenantId && a.ContactId == contactId && a.UserId == userId, ct);
    }

    public IQueryable<T> FilterByUser<T>(IQueryable<T> query, string userId) where T : class
    {
        // This method is used in app services for list filtering.
        // For now: only handle Business/Contact. Other types can be added later.
        if (typeof(T) == typeof(Business))
        {
            return (IQueryable<T>)FilterBusinesses((IQueryable<Business>)query, userId);
        }

        if (typeof(T) == typeof(Contact))
        {
            return (IQueryable<T>)FilterContacts((IQueryable<Contact>)query, userId);
        }

        return query;
    }

    private IQueryable<Business> FilterBusinesses(IQueryable<Business> businesses, string userId)
    {
        var isAdmin = _db.UserRoles
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
            .Any(x => x.UserId == userId && x.Name == "Admin");

        if (isAdmin)
            return businesses;

        return from b in businesses
               join a in _db.BusinessAssignments on b.Id equals a.BusinessId
               where a.UserId == userId
               select b;
    }

    private IQueryable<Contact> FilterContacts(IQueryable<Contact> contacts, string userId)
    {
        var isAdmin = _db.UserRoles
            .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
            .Any(x => x.UserId == userId && x.Name == "Admin");

        if (isAdmin)
            return contacts;

        var user = _db.Users.Single(u => u.Id == userId);

        var assigned =
            from c in contacts
            join a in _db.ContactAssignments on c.Id equals a.ContactId
            where a.UserId == userId
            select c;

        if (user.UserType == UserType.External && user.ExternalContactId.HasValue)
        {
            var self = contacts.Where(c => c.Id == user.ExternalContactId.Value);
            return assigned.Union(self);
        }

        return assigned;
    }

    private async Task<bool> IsAdminAsync(string userId, CancellationToken ct)
    {
        var roleIds = await _db.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync(ct);
        return await _db.Roles.AnyAsync(r => roleIds.Contains(r.Id) && r.Name == "Admin", ct);
    }
}