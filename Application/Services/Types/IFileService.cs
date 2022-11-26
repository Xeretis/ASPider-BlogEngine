using Microsoft.AspNetCore.Http;

namespace Application.Services.Types;

public interface IFileService
{
    string GetUniqueFileName(string fileName);
    Task<string> UploadImageAsync(IFormFile image);
}