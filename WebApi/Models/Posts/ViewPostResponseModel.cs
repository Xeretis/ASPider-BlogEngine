using WebApi.Models.Files;

namespace WebApi.Models.Posts;

public class ViewPostResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public bool Visible { get; set; }
    public bool Approved { get; set; }
    public string? ThumbnailUrl { get; set; }

    public int PageId { get; set; }
    public string AuthorId { get; set; }

    public List<FileUploadResponseModel> Files { get; set; }
}