namespace ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;

public class Scan
{
    public int Id { get; set; }
    public DateTime ScannedAt { get; set; }
    public List<Vulnerabilities> Vulnerabilities { get; set; } = new();
}