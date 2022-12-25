using AutoMapper;
using Domain.Data.Entities;
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

        CreateMap<CreatePostRequestModel, Post>()
            .ForMember(r => r.Files, opt => opt.Ignore());
    }
}