namespace Crm.Infrastructure.Data.Entities;

public sealed class ContactAssignment
{
    public Guid TenantId { get; set; }
    public Guid ContactId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AssignmentType AssignmentType { get; set; }
}