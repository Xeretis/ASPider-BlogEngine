using WebApi.Validation;

namespace WebApi.Models.Image;

public class ImageUploadRequestModel
{
    [MaxFileSize(4 * 1024 * 1024)]
    [AllowedExtensions(new[] { ".jpeg", ".jpg", ".png", ".svg", ".gif", ".ico", ".webp", ".tiff" })]
    public IFormFile Image { get; set; }
}