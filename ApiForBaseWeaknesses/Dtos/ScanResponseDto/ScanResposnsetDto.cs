using System.Data;

namespace ApiForBaseWeaknesses.Dtos.ScanResultDto;

public class ScanResposnsetDto
{
    public int Id { get; set; }
    public DateTime ScannedAt { get; set; }
    public List<VulnerabilitiesDto> Vulnerabilities { get; set; } = new();
}