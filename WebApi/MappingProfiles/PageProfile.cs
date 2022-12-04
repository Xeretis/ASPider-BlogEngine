using AutoMapper;
using Domain.Data.Entities;
using WebApi.MappingProfiles.Resolvers;
using WebApi.Models.Pages;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap<Page, ViewPageResponseModel>()
            .ForMember(r => r.SubPages, o => o.MapFrom<SubpageResolver>());


        CreateMap<CreatePageRequestModel, Page>()
            .ForMember(r => r.Files, opt => opt.Ignore());
    }
}