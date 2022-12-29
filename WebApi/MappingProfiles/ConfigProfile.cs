using AutoMapper;
using Domain.Data;
using WebApi.Models.Config;

namespace WebApi.MappingProfiles;

public class ConfigProfile : Profile
{
    public ConfigProfile()
    {
        CreateMap<EditConfigRequestModel, Config>().ForMember(r => r.AboutFiles, opt => opt.Ignore());
    }
}