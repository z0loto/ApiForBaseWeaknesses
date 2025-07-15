namespace ApiForBaseWeaknesses.Dtos.ScanResultDto;

public class VulnerabilitiesDto
{
    public DateTime Published { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CvssMetricDtos> Metrics { get; set; } = new();
    public List<ReferenceDtos> References { get; set; } = new();
}