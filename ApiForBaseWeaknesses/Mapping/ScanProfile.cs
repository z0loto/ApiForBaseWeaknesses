using ApiForBaseWeaknesses.Dtos.ScanResultDto;
using ApiForBaseWeaknesses.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ApiForBaseWeaknesses.Mapping;

public class ScanProfile : Profile
{
   
    public ScanProfile()
    {
        CreateMap<Scan, ScanResposnsetDto>()
            .ForMember(srd => srd.Vulnerabilities,
                opt => opt
                    .MapFrom(s => s.ScanVulnerability.Select(sv => sv.Vulnerability)));

        CreateMap<Vulnerability, VulnerabilitiesDto>();
        CreateMap<CvssMetric, CvssMetricDtos>();
        CreateMap<Reference, ReferenceDtos>();
    }
}