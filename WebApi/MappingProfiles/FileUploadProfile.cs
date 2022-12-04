using AutoMapper;
using Domain.Data.Entities;
using WebApi.MappingProfiles.Resolvers;
using WebApi.Models.Files;

namespace WebApi.MappingProfiles;

public class FileUploadProfile : Profile
{
    public FileUploadProfile()
    {
        CreateMap<FileUpload, FileUploadResponseModel>().ForMember(r => r.Url, o => o.MapFrom<FileUrlResolver>());
    }
}