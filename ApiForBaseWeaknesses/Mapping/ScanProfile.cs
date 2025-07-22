using ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;
using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Models;
using AutoMapper;
using CvssMetric = ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos.CvssMetric;
using Host = ApiForBaseWeaknesses.Models.Host;
using Reference = ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos.Reference;
using Scan = ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos.Scan;

namespace ApiForBaseWeaknesses.Mapping;

public class ScanProfile : Profile
{
   
    public ScanProfile()
    {
        CreateMap<Models.Scan, Scan>()
            .ForMember(srd => srd.Vulnerabilities,
                opt => opt
                    .MapFrom(s => s.ScanVulnerability.Select(sv => sv.Vulnerability)));

        CreateMap<Vulnerability, Vulnerabilities>();
        CreateMap<Models.CvssMetric, CvssMetric>();
        CreateMap<Models.Reference, Reference>();
        CreateMap<Host, Request>();
        CreateMap<HostDto, Host>();
    }
}