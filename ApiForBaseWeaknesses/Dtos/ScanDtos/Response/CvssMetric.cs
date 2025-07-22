namespace ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;

public class CvssMetric
{
    public string Version { get; set; } = string.Empty;
    public string Vector { get; set; } = string.Empty;
    public double BaseScore { get; set; }
}