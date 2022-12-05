using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Data.Entities;

public class Post : BaseEntity
{
    [Required] [MaxLength(256)] public string Title { get; set; }

    [Required] [MaxLength(256)] public string Description { get; set; }

    [Required] public string Content { get; set; }

    public bool Visible { get; set; }
    public bool Approved { get; set; }
    public string? ThumbnailUrl { get; set; }

    [Required] public Page Page { get; set; }
    [Required] public ApiUser Author { get; set; }
    public string AuthorId { get; set; }

    public List<FileUpload> Files { get; set; }
}