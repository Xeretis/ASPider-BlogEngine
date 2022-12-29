using AutoMapper;
using Domain.Data.Entities;
using WebApi.MappingProfiles.Resolvers;
using WebApi.Models.Approve;
using WebApi.Models.Pages;
using WebApi.Models.Posts;

namespace WebApi.MappingProfiles;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap<Page, ViewPageResponseModel>()
            .ForMember(r => r.SubPages, o => o.MapFrom<SubpageResolver>())
            .ForMember(r => r.Posts, o => o.MapFrom<PostResolver>());
        CreateMap<Page, IndexPageResponseModel>();
        CreateMap<Page, IndexPostPageResponseModel>();
        CreateMap<Page, IndexApprovePageResponseModel>();

        CreateMap<CreatePageRequestModel, Page>()
            .ForMember(r => r.Files, opt => opt.Ignore());
        CreateMap<EditPageRequestModel, Page>()
            .ForMember(r => r.Files, opt => opt.Ignore())
            .ForMember(r => r.ThumbnailUrl, opt => opt.Ignore());
    }
}