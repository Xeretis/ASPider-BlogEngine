using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users;

public class CreatePageRequestModel
{
    [Required] [MaxLength(256)] public string Title { get; set; }
    [Required] [MaxLength(256)] public string Description { get; set; }
    [Required] public string Content { get; set; }

    [Required] public bool Visible { get; set; }
    public string? ThumbnailUrl { get; set; }

    [Required] public int ParentId { get; set; }

    public IFormFile[]? Files { get; set; }
}