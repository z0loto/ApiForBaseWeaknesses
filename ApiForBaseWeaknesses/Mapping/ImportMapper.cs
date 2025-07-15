using System.Net;
using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Dto;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Mapping;

public class ImportMapper
{
    public static List<Vulnerability> MapToListVulnerability(MainVulnerabilitiesDto? vulnerabilitiesDto)
    {
        return vulnerabilitiesDto.Vulnerabilities
            .Where(v => v.Cve != null)
            .Select(v => MapToVulnerability(v.Cve))
            .ToList();
    }

    private static Vulnerability MapToVulnerability(CveDto? dto)
    {
        if (dto == null) return null;

        return new Vulnerability
        {
            Name = dto.Id,
            Published = DateTime.SpecifyKind(dto.Published, DateTimeKind.Utc),
            Status = dto.VulnStatus,
            Description = dto.Descriptions?.FirstOrDefault(d => d.Lang == "en")?.Value ?? "No description",
            CvssMetrics = MapToCvssMetric(dto.Metrics),
            References = dto.References?.Select(r => new Reference
            {
                Url = r.Url,
                Source = r.Source
            }).ToList() ?? new()
        };
    }

    private static List<CvssMetric> MapToCvssMetric(MetricsDto? dto)
    {
        if (dto == null) return null;
        List<CvssMetric> result = new();

        void Add(ICollection<CvssMetricDto> list)
        {
            if (list.Count == 0)
            {
                return;
            }

            foreach (var cvss in list)
            {
                result.Add(new CvssMetric
                {
                    Vector = cvss.CvssData.VectorString,
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

    public static List<Host> MapToHost(List<string> list)
    {
        return list.Where(l => l != null && IPAddress.TryParse(l.Trim(), out _)).Select(l => new Host
        {
            Ip = l
        }).ToList();
    }


}