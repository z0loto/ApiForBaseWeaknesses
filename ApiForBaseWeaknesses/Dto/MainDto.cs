namespace ApiForBaseWeaknesses.Dto;

public class MainDto
{
    public ICollection<VulnerabilitiesDto> Vulnerabilities { get; set; } = new List<VulnerabilitiesDto>();
}