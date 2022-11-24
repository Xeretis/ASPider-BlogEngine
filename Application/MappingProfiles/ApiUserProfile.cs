using AutoMapper;
using Domain.Data.Entities;
using Domain.Data.Models.Auth;

namespace Application.MappingProfiles;

public class ApiUserProfile : Profile
{
    public ApiUserProfile()
    {
        CreateMap<ApiUser, LoginResponseUserModel>();
    }
}