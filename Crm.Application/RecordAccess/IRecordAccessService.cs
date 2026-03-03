namespace Crm.Application.RecordAccess;

public interface IRecordAccessService
{
    Task<bool> CanAccessBusinessAsync(string userId, Guid businessId, CancellationToken ct = default);
    Task<bool> CanAccessContactAsync(string userId, Guid contactId, CancellationToken ct = default);

    IQueryable<T> FilterByUser<T>(IQueryable<T> query, string userId) where T : class;
}