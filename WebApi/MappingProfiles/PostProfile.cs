using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, ViewUserPostResponseModel>();
    }
}