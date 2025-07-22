namespace ApiForBaseWeaknesses.Dtos.ImportDtos;

public class MainVulnerabilities
{
    public ICollection<Vulnerabilities> Vulnerabilities { get; set; } = new List<Vulnerabilities>();
}