using System.ComponentModel.DataAnnotations;
using WebApi.Validation;

namespace WebApi.Models.Config;

public class EditConfigRequestModel
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string AboutContent { get; set; }
    public string? PageIconUrl { get; set; }
    public string? FaviconUrl { get; set; }

    [AllowedExtensions(new[]
    {
        ".pptx", ".ppt", ".xlsx", ".xls", ".docx", ".doc", ".zip", ".pdf", ".jpeg", ".jpg", ".png", ".svg", ".gif",
        ".ico", ".webp", ".tiff"
    })]
    [MaxFileSize(25 * 1024 * 1024)]
    public IFormFile[]? AboutFiles { get; set; }

    [Required] public string ContactEmail { get; set; }
    [Required] public string ContactPhone { get; set; }
    [Required] public string ContactName { get; set; }
}