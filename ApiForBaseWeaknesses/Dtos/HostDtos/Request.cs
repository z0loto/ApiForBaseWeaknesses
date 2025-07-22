namespace ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;

public class Request
{
    public ICollection<int> Indexes { get; set; } = new List<int>();
}