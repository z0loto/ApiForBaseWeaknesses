namespace ApiForBaseWeaknesses.Models;

public class Scan
{
    public int Id { get; set; }
    public DateTime ScannedAt { get; set; } = DateTime.UtcNow;
    public int HostId { get; set; }
    public virtual Host Host { get; set; } = new();
    public virtual ICollection<ScanVulnerability> ScanVulnerability { get; set; } = new List<ScanVulnerability>();
}