namespace ApiForBaseWeaknesses.Requests;

public class TopHosts
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int VulnerabilityCount { get; set; }
}