namespace Crm.Application.Common;

public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}