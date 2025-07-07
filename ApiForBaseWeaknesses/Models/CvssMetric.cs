using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiForBaseWeaknesses.Models
{
    public class CvssMetric
    {
        public int Id { get; set; }
        public string Version { get; set; } = string.Empty;
        public string VectorString { get; set; } = string.Empty;
        public double? BaseScore { get; set; }
        public int VulnerabilityId { get; set; }
        public virtual Vulnerability Vulnerability { get; set; }
    }
}
