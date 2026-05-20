using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class BackgroundJobLog : BaseEntity
{
    public string JobName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
}
