using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dto;

public class Metrics
{

    public CvssMetricDto CvssMetricV40 { get; set; } = new();
    public CvssMetricDto CvssMetricV30 { get; set; } = new();
    public CvssMetricDto CvssMetricV31 { get; set; } = new();
    public CvssMetricDto CvssMetricV2 { get; set; } = new();

}