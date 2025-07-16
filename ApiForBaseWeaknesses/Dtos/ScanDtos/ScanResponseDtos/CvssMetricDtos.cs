namespace ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;

public class CvssMetricDtos
{
    public string Version { get; set; } = string.Empty;
    public string Vector { get; set; } = string.Empty;
    public double BaseScore { get; set; }
}