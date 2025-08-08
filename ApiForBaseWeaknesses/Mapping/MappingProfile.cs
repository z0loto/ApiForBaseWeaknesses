using AutoMapper;
using Host = ApiForBaseWeaknesses.Dtos.Host;

namespace ApiForBaseWeaknesses.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Host, Models.Host>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Scans, opt => opt.Ignore());

        CreateMap<Models.Host, Host>();
    }
}