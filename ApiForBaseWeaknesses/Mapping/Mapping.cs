using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Dto;

namespace ApiForBaseWeaknesses.Mapping;

public class Mapping
{
    // public static Vulnerability MapToEntity(VulnerabilitiesDto dto)
    // {
    //     if (dto == null) return null;
    //
    //     return new Vulnerability
    //     {
    //         Id = dto.Id, // если есть
    //         CveId = dto.CveId,
    //         Published = dto.Published,
    //         Description = dto.Description,
    //         CvssMetrics = dto.CvssMetrics?.Select(MapToEntity).ToList()
    //     };
    // }
    //
    // public static CvssMetric MapToEntity(CvssMetricDto dto)
    // {
    //     if (dto == null) return null;
    //
    //     return new CvssMetric
    //     {
    //         Id = dto.Id,
    //         Source = dto.Source,
    //         Type = dto.Type,
    //         // ...
    //     };
    // }
}