using Application.Services.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Implementations;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string GetUniqueFileName(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        return string.Concat(Path.GetFileNameWithoutExtension(fileName)
            , "_"
            , Guid.NewGuid().ToString().AsSpan(0, 8)
            , Path.GetExtension(fileName));
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        var uniqueFileName = GetUniqueFileName(image.FileName);
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images");
        var filePath = Path.Combine(uploads, uniqueFileName);
        await image.CopyToAsync(new FileStream(filePath, FileMode.Create));

        return Path.Combine("Resources", "Images", uniqueFileName);
    }
}