namespace Crm.Domain.CRM;

public sealed class BusinessContact
{
    public Guid TenantId { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public Guid ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
}