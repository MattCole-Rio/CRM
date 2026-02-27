using Crm.Domain.Common;

namespace Crm.Domain.CRM;

public sealed class Business : TenantEntity
{
    private Business() { }

    public Business(Guid tenantId, string name)
        : base(tenantId)
    {
        SetName(name);
    }

    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }

    public Guid? BusinessStatusId { get; private set; }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Business name is required.", nameof(name));

        Name = name.Trim();
        Touch();
    }

    public void SetContactInfo(string? email, string? phone, string? website)
    {
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        Website = string.IsNullOrWhiteSpace(website) ? null : website.Trim();
        Touch();
    }

    public void SetStatus(Guid? statusId)
    {
        BusinessStatusId = statusId;
        Touch();
    }
}
