namespace Crm.Application.Permissions;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string permissionKey, CancellationToken ct = default);
}