namespace ApiForBaseWeaknesses.Models
{
    public class CvssMetric
    {
        public int Id { get; set; }
        public string Version { get; set; } = string.Empty;
        public string Vector { get; set; } = string.Empty;
        public double? BaseScore { get; set; }
        public int VulnerabilityId { get; set; }
        public virtual Vulnerability Vulnerability { get; set; }
    }
}