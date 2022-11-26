using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Auth;

namespace WebApi.MappingProfiles;

public class ApiUserProfile : Profile
{
    public ApiUserProfile()
    {
        CreateMap<ApiUser, LoginResponseUserModel>();
        CreateMap<ApiUser, UserResponseModel>();
    }
}