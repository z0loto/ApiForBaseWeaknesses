namespace ApiForBaseWeaknesses.Models;

public class Host
{
    public int Id { get; set; }
    public string Ip { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Scan> Scans { get; set; } = new List<Scan> ();
}