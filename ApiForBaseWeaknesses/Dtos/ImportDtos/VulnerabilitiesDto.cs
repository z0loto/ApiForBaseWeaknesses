using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Dto
{
    public class VulnerabilitiesDto
    {
        public CveDto? Cve { get; set; } = new();
    }
}