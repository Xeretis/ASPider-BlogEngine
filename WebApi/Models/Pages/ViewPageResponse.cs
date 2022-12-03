using Domain.Data.Entities;

namespace WebApi.Models.Pages;

public class ViewPageResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public bool Visible { get; set; }
    public string? ThumbnailUrl { get; set; }

    public int? ParentId { get; set; }
    public string? CreatorId { get; set; }

    public List<ViewPageResponse> SubPages { get; set; }
    public List<Post> Posts { get; set; }
    public List<FileUpload> Files { get; set; }
}