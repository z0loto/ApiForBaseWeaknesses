namespace ApiForBaseWeaknesses.Dtos.Import;

public class MainVulnerabilities
{
    public ICollection<Vulnerabilities> Vulnerabilities { get; set; } = new List<Vulnerabilities>();
}