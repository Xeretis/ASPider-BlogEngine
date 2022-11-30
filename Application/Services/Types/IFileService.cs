using Domain.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Types;

public interface IFileService
{
    string GetUniqueFileName(string fileName);
    Task<string> UploadImageAsync(IFormFile image);
    Task<FileUpload> UploadFileAsync(IFormFile file);
}