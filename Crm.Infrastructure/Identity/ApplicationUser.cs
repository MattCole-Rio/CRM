using Microsoft.AspNetCore.Identity;

namespace Crm.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public Guid TenantId { get; set; }

    public UserType UserType { get; set; } = UserType.External;

    // For staff mapping (Entra). Optional for now.
    public Guid? EntraObjectId { get; set; }

    // External user must be linked to a contact (can be null during invite, but then access is restricted).
    public Guid? ExternalContactId { get; set; }

    // Manager relationship (for approvals and manager record access)
    public string? ManagerUserId { get; set; }
    public ApplicationUser? ManagerUser { get; set; }
}