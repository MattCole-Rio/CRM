namespace Crm.Application.CRM.DTOs;

public sealed record BusinessDto(
    Guid Id,
    string Name,
    string? Email,
    string? Phone,
    string? Website
);