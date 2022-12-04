using Application.Services.Types;
using Domain.Data.Entities;
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

        return Path.Combine("Images", uniqueFileName);
    }

    public async Task<FileUpload> UploadFileAsync(IFormFile file)
    {
        var uniqueFileName = GetUniqueFileName(file.FileName);
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Files");
        var filePath = Path.Combine(uploads, uniqueFileName);
        await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

        return new FileUpload
        {
            Filename = uniqueFileName,
            OriginalFilename = file.FileName,
            ContentType = file.ContentType
        };
    }
}