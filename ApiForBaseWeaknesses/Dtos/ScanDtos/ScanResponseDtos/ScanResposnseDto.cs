namespace ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;

public class ScanResposnseDto
{
    public int Id { get; set; }
    public DateTime ScannedAt { get; set; }
    public List<VulnerabilitiesDto> Vulnerabilities { get; set; } = new();
}