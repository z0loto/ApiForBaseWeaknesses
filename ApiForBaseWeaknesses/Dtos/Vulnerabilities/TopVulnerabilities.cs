namespace ApiForBaseWeaknesses.Dtos.Vulnerabilities;

public class TopVulnerabilities
{
    public int Id { get; set; }
    public DateTime Published { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Count { get; set; }
}