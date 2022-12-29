using System.ComponentModel.DataAnnotations;
using Application.Services.Types;
using WebApi.Validation;

namespace WebApi.Models.Posts;

public class EditPostRequestModel : IPostService.IPostModification
{
    [Required] public bool Visible { get; set; }

    [AllowedExtensions(new[]
    {
        ".pptx", ".ppt", ".xlsx", ".xls", ".docx", ".doc", ".zip", ".pdf", ".jpeg", ".jpg", ".png", ".svg", ".gif",
        ".ico", ".webp", ".tiff"
    })]
    [MaxFileSize(25 * 1024 * 1024)]
    public IFormFile[]? Files { get; set; }

    [Required] [MaxLength(256)] public string Title { get; set; }
    [Required] [MaxLength(256)] public string Description { get; set; }
    [Required] public string Content { get; set; }

    public string? ThumbnailUrl { get; set; }

    [Required] public int PageId { get; set; }
}