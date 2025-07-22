namespace ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;

public class Vulnerabilities
{
    public DateTime Published { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CvssMetric> Metrics { get; set; } = new();
    public List<Reference> References { get; set; } = new();
}