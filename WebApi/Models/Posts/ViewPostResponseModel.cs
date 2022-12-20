using WebApi.Models.Files;

namespace WebApi.Models.Posts;

public class ViewPostResponseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int PageId { get; set; }
    public ViewPostUserResponseModel Author { get; set; }

    public List<FileUploadResponseModel> Files { get; set; }
}

public class ViewPostUserResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}