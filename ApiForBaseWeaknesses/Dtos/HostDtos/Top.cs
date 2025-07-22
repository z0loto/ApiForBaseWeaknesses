namespace ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;

public class Top
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int VulnerabilityCount { get; set; }
}