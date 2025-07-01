using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiForBaseWeaknesses.Models
{
    public class References
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }=string.Empty;
        public string Source { get; set; } = string.Empty;
        [Column("Vulnerability_Id")]
        public int VulnerabilityId { get; set; }
        public virtual Vulnerability Vulnerability { get; set; } = null!;
    }
}
