namespace ApiForBaseWeaknesses.Dtos.ImportDtos;

public class CvssData
{
    public string Version { get; set; } = string.Empty;
    public string VectorString { get; set; } = string.Empty;
    public double BaseScore { get; set; } = new();
}