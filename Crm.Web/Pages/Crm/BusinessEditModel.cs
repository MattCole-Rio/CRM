using System.ComponentModel.DataAnnotations;

namespace Crm.Web.Pages.Crm;

public sealed class BusinessEditModel
{
    [Required, StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }
    public string? Website { get; set; }
}