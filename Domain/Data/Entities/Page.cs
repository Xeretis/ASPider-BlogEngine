using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Data.Entities;

public class Page : BaseEntity
{
    [Required] [MaxLength(256)] public string Title { get; set; }
    [Required] [MaxLength(256)] public string Description { get; set; }
    [Required] public string Content { get; set; }

    public bool Visible { get; set; }
    public string? ThumbnailUrl { get; set; }

    public Page? Parent { get; set; }
    public List<Page> Children { get; set; }

    public ApiUser? Creator { get; set; }

    public List<Post> Posts { get; set; }
    public List<FileUpload> Files { get; set; }
}