using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Approve;
using WebApi.Models.Auth;
using WebApi.Models.Pages;
using WebApi.Models.Posts;
using WebApi.Models.Users;

namespace WebApi.MappingProfiles;

public class ApiUserProfile : Profile
{
    public ApiUserProfile()
    {
        CreateMap<ApiUser, LoginResponseUserModel>();
        CreateMap<ApiUser, UserResponseModel>();
        CreateMap<ApiUser, UsersIndexResponseModel>();
        CreateMap<ApiUser, ViewUserResponseModel>();
        CreateMap<ApiUser, ViewPageUserResponseModel>();
        CreateMap<ApiUser, IndexPageUserResponseModel>();
        CreateMap<ApiUser, ViewPostUserResponseModel>();
        CreateMap<ApiUser, IndexPostUserResponseModel>();
        CreateMap<ApiUser, IndexApproveUserResponseModel>();
        CreateMap<ApiUser, ViewApproveUserResponseModel>();

        CreateMap<CreateUserRequestModel, ApiUser>();
        CreateMap<EditUserRequestModel, ApiUser>();
        CreateMap<EditSelfRequestModel, ApiUser>();
    }
}