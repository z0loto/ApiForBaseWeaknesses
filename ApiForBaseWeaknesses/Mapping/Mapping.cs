using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Dto;

namespace ApiForBaseWeaknesses.Mapping;

public class Mapping
{
    public static List<Vulnerability> MapToListVulnerability(MainDto dto)
    {
        return dto.Vulnerabilities
            .Where(v => v.Cve != null)
            .Select(v => MapToVulnerability(v.Cve))
            .ToList();
    }
    public static Vulnerability MapToVulnerability(CveDto dto)
    {
        if (dto == null) return null;
    
        return new Vulnerability
        {
            Name = dto.Id,
            Published = dto.Published,
            Status = dto.VulnStatus,
            Description = dto.Descriptions?.FirstOrDefault(d=>d.Lang=="en")?.Value ?? "No description",
            CvssMetric =MapToCvssMetric(dto.Metrics),
            Reference = dto.References?.Select(r=>new References
            {
                Url=r.Url,
                Source=r.Source
            }).ToList() ?? new()
        };
    }
    
    public static List<CvssMetric> MapToCvssMetric(MetricsDto dto)
    {
        if (dto == null) return null;
        List<CvssMetric> result = new();
        void Add(List<CvssMetricDto> list)
        {
            if (list.Count==0)
            {
                return; 
            } 
            foreach(var cvss in list)
            {
                result.Add(new CvssMetric
                {
                    VectorString = cvss.CvssData.VectorString,
                    Version = cvss.CvssData.Version,
                    BaseScore = cvss.CvssData?.BaseScore
                }); 
            }
          

        }
        Add(dto.CvssMetricV2);
        Add(dto.CvssMetricV30);
        Add(dto.CvssMetricV31);
        Add(dto.CvssMetricV40);


        return result;
    }
}