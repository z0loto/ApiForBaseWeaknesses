namespace ApiForBaseWeaknesses.Dtos.Import;

public class Metrics
{
    public ICollection<CvssMetric> CvssMetricV40 { get; set; } = new List<CvssMetric>();
    public ICollection<CvssMetric> CvssMetricV30 { get; set; } = new List<CvssMetric>();
    public ICollection<CvssMetric> CvssMetricV31 { get; set; } = new List<CvssMetric>();
    public ICollection<CvssMetric> CvssMetricV2 { get; set; } = new List<CvssMetric>();
}