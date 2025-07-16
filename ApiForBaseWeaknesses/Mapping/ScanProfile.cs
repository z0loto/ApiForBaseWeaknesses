using ApiForBaseWeaknesses.Dtos.ScanDto.ScanRequestDto;
using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Models;
using AutoMapper;
using Host = ApiForBaseWeaknesses.Models.Host;

namespace ApiForBaseWeaknesses.Mapping;

public class ScanProfile : Profile
{
   
    public ScanProfile()
    {
        CreateMap<Scan, ScanResposnseDto>()
            .ForMember(srd => srd.Vulnerabilities,
                opt => opt
                    .MapFrom(s => s.ScanVulnerability.Select(sv => sv.Vulnerability)));

        CreateMap<Vulnerability, VulnerabilitiesDto>();
        CreateMap<CvssMetric, CvssMetricDtos>();
        CreateMap<Reference, ReferenceDtos>();
        CreateMap<Host, HostRequestDto>();
    }
}