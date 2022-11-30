using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap<CreatePageRequestModel, Page>().ForMember(r => r.Files, opt => opt.Ignore());
    }
}