
using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dto;

public class CveDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Published { get; init; } = DateTime.UtcNow;
    public string VulnStatus { get; set; } = string.Empty;
    public ICollection <DescriptionsDto> Descriptions { get; set; } = new List<DescriptionsDto>();
    public MetricsDto Metrics { get; set; } = new();
    public ICollection<ReferencesDto> References { get; set; } = new List<ReferencesDto>();
}