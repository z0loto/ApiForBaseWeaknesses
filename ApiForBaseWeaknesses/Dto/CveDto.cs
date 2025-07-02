
using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dto;

public class CveDto
{
    public string Id { get; set; } = string.Empty;
    public string Published { get; set; } = string.Empty;
    public string VulnStatus { get; set; } = string.Empty;
    public List<DescriptionsDto> Descriptions { get; set; } = new();// выбрать по языку
    public MetricsDto Metrics { get; set; } = new();
    public List<ReferencesDto> References { get; set; } = new ();
}