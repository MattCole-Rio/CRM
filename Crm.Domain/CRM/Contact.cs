using Crm.Domain.Common;

namespace Crm.Domain.CRM;

public sealed class Contact : TenantEntity
{
    private Contact() { }

    public Contact(Guid tenantId, string firstName, string lastName)
        : base(tenantId)
    {
        SetName(firstName, lastName);
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    public Guid? ContactStatusId { get; private set; }

    public void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Touch();
    }

    public void SetContactInfo(string? email, string? phone)
    {
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        Touch();
    }

    public void SetStatus(Guid? statusId)
    {
        ContactStatusId = statusId;
        Touch();
    }
}
