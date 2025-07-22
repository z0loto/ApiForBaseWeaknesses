using ApiForBaseWeaknesses.Dtos.HostDtos.ScanRequestDto;
using AutoMapper;

namespace ApiForBaseWeaknesses.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HostDto, Models.Host>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Scans, opt => opt.Ignore());

        CreateMap<Models.Host, HostDto>();
    }
}