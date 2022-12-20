using AutoMapper;
using Domain.Data.Entities;
using WebApi.MappingProfiles.Resolvers;
using WebApi.Models.Pages;

namespace WebApi.MappingProfiles;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap<Page, ViewPageResponseModel>()
            .ForMember(r => r.SubPages, o => o.MapFrom<SubpageResolver>())
            .ForMember(r => r.Posts, o => o.MapFrom<PostResolver>());
        CreateMap<Page, IndexPageResponseModel>();

        CreateMap<CreatePageRequestModel, Page>()
            .ForMember(r => r.Files, opt => opt.Ignore());
        CreateMap<EditPageRequestModel, Page>()
            .ForMember(r => r.Files, opt => opt.Ignore())
            .ForMember(r => r.ThumbnailUrl, opt => opt.Ignore());
    }
}