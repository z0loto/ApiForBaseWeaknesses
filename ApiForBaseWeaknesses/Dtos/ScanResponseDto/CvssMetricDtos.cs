namespace ApiForBaseWeaknesses.Dtos.ScanResultDto;

public class CvssMetricDtos
{
    public string Version { get; set; } = string.Empty;
    public string Vector { get; set; } = string.Empty;
    public double BaseScore { get; set; }
}