using WebApi.Models.Files;

namespace WebApi.Models.Posts;

public class IndexPostResponseModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }

    public bool Visible { get; set; }
    public bool Approved { get; set; }

    public IndexPostPageResponseModel Page { get; set; }
    public IndexPostUserResponseModel Author { get; set; }

    public List<FileUploadResponseModel> Files { get; set; }
}

public class IndexPostUserResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

public class IndexPostPageResponseModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public bool Visible { get; set; }

    public string? ThumbnailUrl { get; set; }
}