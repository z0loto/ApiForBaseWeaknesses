namespace ApiForBaseWeaknesses.Dtos.ImportDtos;

public class MainVulnerabilitiesDto
{
    public ICollection<VulnerabilitiesDto> Vulnerabilities { get; set; } = new List<VulnerabilitiesDto>();
}