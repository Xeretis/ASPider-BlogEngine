using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Auth;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class ApiUserProfile : Profile
{
    public ApiUserProfile()
    {
        CreateMap<ApiUser, LoginResponseUserModel>();
        CreateMap<ApiUser, UserResponseModel>();
        CreateMap<ApiUser, UsersIndexResponseModel>();

        CreateMap<CreateUserRequestModel, ApiUser>();
    }
}