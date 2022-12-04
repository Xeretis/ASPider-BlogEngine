using AutoMapper;
using Domain.Data.Entities;
using WebApi.Models.Files;

namespace WebApi.MappingProfiles.Resolvers;

public class FileUrlResolver : IValueResolver<FileUpload, FileUploadResponseModel, string>
{
    public string Resolve(FileUpload source, FileUploadResponseModel destination, string destMember,
        ResolutionContext context)
    {
        return $"https://localhost:7003/Files/{source.Filename}";
    }
}