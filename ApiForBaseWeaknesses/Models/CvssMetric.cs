using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiForBaseWeaknesses.Models
{
    public class CvssMetric
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Version { get; set; } = string.Empty;
        [Required]
        [Column("Vector_String")]
        public string VectorString { get; set; } = string.Empty;
        [Column("Base_Score")]
        public double? BaseScore { get; set; }
        [Column("Vulnerability_Id")]
        public int VulnerabilityId { get; set; }
        public virtual Vulnerability Vulnerability { get; set; } = null!;
        //null! можно не использовать
    }
}
