
namespace ApiForBaseWeaknesses.Dto;

public class CveDto
{
    //[JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    //[JsonPropertyName("published")]
    public string Published { get; set; } = string.Empty;
    //[JsonPropertyName("vulnStatus")]
    public string VulnStatus { get; set; } = string.Empty;
    public DescriptionsDto Descriptions { get; set; } = new();// выбрать по языку
    public MetricsDto Metrics { get; set; } = new();
    public List<ReferencesDto> References { get; set; } = new List<ReferencesDto>();
}