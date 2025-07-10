namespace ApiForBaseWeaknesses.Dto;

public class MainVulnerabilitiesDto
{
    public ICollection<VulnerabilitiesDto> Vulnerabilities { get; set; } = new List<VulnerabilitiesDto>();
}