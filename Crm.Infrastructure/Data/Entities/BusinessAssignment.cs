namespace Crm.Infrastructure.Data.Entities;

public enum AssignmentType
{
    AccountManager = 1,
    Contributor = 2,
    Viewer = 3
}

public sealed class BusinessAssignment
{
    public Guid TenantId { get; set; }
    public Guid BusinessId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AssignmentType AssignmentType { get; set; }
}