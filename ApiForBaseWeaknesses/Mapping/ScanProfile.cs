using ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos;
using ApiForBaseWeaknesses.Models;
using ApiForBaseWeaknesses.Requests;
using ApiForBaseWeaknesses.Responses.ForScans;
using AutoMapper;
using CvssMetric = ApiForBaseWeaknesses.Responses.ForScans.CvssMetric;
using Host = ApiForBaseWeaknesses.Models.Host;
using Reference = ApiForBaseWeaknesses.Dtos.ScanDtos.ScanResponseDtos.Reference;
using Scan = ApiForBaseWeaknesses.Responses.ForScans.Scan;

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
        CreateMap<Host, HostIndexes>();
        CreateMap<Dtos.Host, Host>();
    }
}