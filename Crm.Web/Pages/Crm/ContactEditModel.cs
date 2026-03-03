using System.ComponentModel.DataAnnotations;

namespace Crm.Web.Pages.Crm;

public sealed class ContactEditModel
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }
}