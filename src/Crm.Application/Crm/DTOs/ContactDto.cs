namespace Crm.Application.CRM.DTOs;

public sealed record ContactDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone
);