namespace ApiForBaseWeaknesses.Dtos.ScanDto.ScanRequestDto;

public class HostRequestDto
{
    public int Id { get; set; }
    public string Ip { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}