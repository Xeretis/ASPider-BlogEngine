using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Approve;
using WebApi.Models.Pages;
using WebApi.Models.Posts;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, ViewUserPostResponseModel>();
        CreateMap<Post, ViewPagePostResponseModel>();
        CreateMap<Post, ViewPostResponseModel>();
        CreateMap<Post, IndexPostResponseModel>();
        CreateMap<Post, IndexApproveResponseModel>();
        CreateMap<Post, ViewApproveResponseModel>();

        CreateMap<CreatePostRequestModel, Post>()
            .ForMember(r => r.Files, opt => opt.Ignore());
        CreateMap<EditPostRequestModel, Post>()
            .ForMember(r => r.Files, opt => opt.Ignore())
            .ForMember(r => r.ThumbnailUrl, opt => opt.Ignore());
        CreateMap<EditApproveRequestModel, Post>();
    }
}