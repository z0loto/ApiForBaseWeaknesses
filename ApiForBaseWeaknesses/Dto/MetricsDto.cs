using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dto;

public class MetricsDto
{

    public List<CvssMetricDto> CvssMetricV40 { get; set; } = new List<CvssMetricDto>();
    public List<CvssMetricDto> CvssMetricV30 { get; set; } = new List<CvssMetricDto>();
    public List<CvssMetricDto> CvssMetricV31 { get; set; } = new List<CvssMetricDto>();
    public List<CvssMetricDto> CvssMetricV2 { get; set; } = new List<CvssMetricDto>();

}