using WebApi.Models.Files;

namespace WebApi.Models.Pages;

public class IndexPageResponseModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public bool Visible { get; set; }
    public string? ThumbnailUrl { get; set; }

    public int? ParentId { get; set; }
    public string? CreatorId { get; set; }

    public List<FileUploadResponseModel> Files { get; set; }
}