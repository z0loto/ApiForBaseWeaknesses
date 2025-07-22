namespace ApiForBaseWeaknesses.Dtos.ImportDtos;

public class Cve
{
    public string Id { get; set; } = string.Empty;
    public DateTime Published { get; init; } = DateTime.UtcNow;
    public string VulnStatus { get; set; } = string.Empty;
    public ICollection<Descriptions> Descriptions { get; set; } = new List<Descriptions>();
    public Metrics Metrics { get; set; } = new();
    public ICollection<References> References { get; set; } = new List<References>();
}