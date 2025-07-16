namespace ApiForBaseWeaknesses.Dtos.ImportDtos;

public class MetricsDto
{
    public ICollection<CvssMetricDto> CvssMetricV40 { get; set; } = new List<CvssMetricDto>();
    public ICollection<CvssMetricDto> CvssMetricV30 { get; set; } = new List<CvssMetricDto>();
    public ICollection<CvssMetricDto> CvssMetricV31 { get; set; } = new List<CvssMetricDto>();
    public ICollection<CvssMetricDto> CvssMetricV2 { get; set; } = new List<CvssMetricDto>();
}