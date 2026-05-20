using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
